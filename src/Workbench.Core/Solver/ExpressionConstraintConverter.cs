using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Repeaters;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Convert the expression constraint model representation into a representation usable 
    /// by the or-tools solver.
    /// </summary>
    class ExpressionConstraintConverter
    {
        private readonly OrToolsCache cache;
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private readonly ModelModel model;

        /// <summary>
        /// Initialize the expression constraint converter with a solver and or-tools cache.
        /// </summary>
        /// <param name="theSolver">Google or-tools solver instance.</param>
        /// <param name="theCache">Cache mapping between the model and Google or-tools solver.</param>
        /// <param name="theModel">Model</param>
        public ExpressionConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            Contract.Requires<ArgumentNullException>(theCache != null);
            this.solver = theSolver;
            this.cache = theCache;
            this.model = theModel;
        }

        /// <summary>
        /// Map the expression constraint model into the or-tools solver.
        /// </summary>
        /// <param name="constraint">Expression constraint model.</param>
        public void ProcessConstraint(ExpressionConstraintGraphicModel constraint)
        {
            Contract.Requires<ArgumentNullException>(constraint != null);
            var repeater = new ConstraintRepeater(this.solver, this.cache, this.model);
            repeater.Process(repeater.CreateContextFrom(constraint));
        }
    }
}
