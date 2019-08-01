using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Parsers;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Convert the model constraints into a representation that works in or-tools solver.
    /// </summary>
    internal class OrConstraintConverter
    {
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private readonly OrToolsCache cache;
        private readonly OrValueMapper valueMapper;

        /// <summary>
        /// Initialize the constraint converter with a Google or-tools solver and a or-tools cache.
        /// </summary>
        internal OrConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache, OrValueMapper theValueMapper)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            Contract.Requires<ArgumentNullException>(theCache != null);
            Contract.Requires<ArgumentNullException>(theValueMapper != null);

            this.solver = theSolver;
            this.cache = theCache;
            this.valueMapper = theValueMapper;
        }

        /// <summary>
        /// Process the constraints from the model.
        /// </summary>
        /// <param name="model">The model.</param>
        internal void ProcessConstraints(ModelModel model)
        {
            Contract.Requires<ArgumentNullException>(model != null);

            foreach (var constraint in model.Constraints)
            {
                switch (constraint)
                {
                    case ExpressionConstraintModel expressionConstraint:
                        var expressionConstraintConverter = new OrExpressionConstraintConverter(this.solver, this.cache, model, this.valueMapper);
                        expressionConstraintConverter.ProcessConstraint(expressionConstraint);
                        break;

                    case AllDifferentConstraintModel allDifferentConstraint:
                        var allDifferentConstraintConverter = new OrAllDifferentConstraintConverter(this.solver, this.cache, model);
                        allDifferentConstraintConverter.ProcessConstraint(allDifferentConstraint);
                        break;

                    default:
                        throw new NotImplementedException("Unknown constraint.");
                }
            }

            // Constraints inside bundles must be processed after the bucket maps have been created
            foreach (var bucket in model.Buckets)
            {
                foreach (var allDifferentConstraint in bucket.Bundle.AllDifferentConstraints)
                {
                    var variableNames = new List<string>(ExtractVariablesFrom(allDifferentConstraint.Expression.Text));

                    for (var bundleCounter = 0; bundleCounter < bucket.Size; bundleCounter++)
                    {
                        var bucketName = bucket.Name.Text;
                        var expressionText = $"%{bucketName}[{bundleCounter}].{variableNames[0]} <> %{bucketName}[{bundleCounter}].{variableNames[1]}";
                        var expressionConstraint = new ExpressionConstraintModel(model, new ConstraintExpressionModel(expressionText));
                        var expressionConstraintConverter = new OrExpressionConstraintConverter(this.solver, this.cache, model, this.valueMapper);
                        expressionConstraintConverter.ProcessConstraint(expressionConstraint);
                    }
                }
            }
        }

        private IEnumerable<string> ExtractVariablesFrom(string expressionText)
        {
            var x = Array.ConvertAll(expressionText.Split(','), variableName => variableName.Trim());
            Debug.Assert(x.Length > 0);

            return x;
        }
    }
}
