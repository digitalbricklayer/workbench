using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Convert the model constraints into a representation that works in or-tools solver.
    /// </summary>
    internal class ConstraintConverter
    {
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private readonly OrToolsCache cache;

        /// <summary>
        /// Initialize the constraint converter with a Google or-tools solver and a or-tools cache.
        /// </summary>
        internal ConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver,
                                OrToolsCache theCache)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            Contract.Requires<ArgumentNullException>(theCache != null);

            this.solver = theSolver;
            this.cache = theCache;
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
                    case ExpressionConstraintGraphicModel expressionConstraint:
                        var expressionConstraintConverter = new ExpressionConstraintConverter(this.solver, this.cache, theModel);
                        expressionConstraintConverter.ProcessConstraint(expressionConstraint);
                        break;

                    case AllDifferentConstraintGraphicModel allDifferentConstraint:
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
