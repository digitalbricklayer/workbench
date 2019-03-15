using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Convert the model constraints into a representation that works in or-tools solver.
    /// </summary>
    internal class OrConstraintConverter
    {
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private readonly OrToolsCache cache;
        private readonly ValueMapper valueMapper;

        /// <summary>
        /// Initialize the constraint converter with a Google or-tools solver and a or-tools cache.
        /// </summary>
        internal OrConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache, ValueMapper theValueMapper)
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
        /// <param name="theModel">The model.</param>
        internal void ProcessConstraints(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            foreach (var constraint in theModel.Constraints)
            {
                switch (constraint)
                {
                    case ExpressionConstraintModel expressionConstraint:
                        var expressionConstraintConverter = new OrExpressionConstraintConverter(this.solver, this.cache, theModel, this.valueMapper);
                        expressionConstraintConverter.ProcessConstraint(expressionConstraint);
                        break;

                    case AllDifferentConstraintModel allDifferentConstraint:
                        var allDifferentConstraintConverter = new AllDifferentConstraintConverter(this.solver, this.cache, theModel);
                        allDifferentConstraintConverter.ProcessConstraint(allDifferentConstraint);
                        break;

                    default:
                        throw new NotImplementedException("Unknown constraint.");
                }
            }
        }
    }
}
