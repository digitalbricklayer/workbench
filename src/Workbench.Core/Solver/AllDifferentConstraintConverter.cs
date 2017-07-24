using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Convert the all different constraint model representation into a representation usable 
    /// by the or-tools solver.
    /// </summary>
    class AllDifferentConstraintConverter
    {
        private readonly OrToolsCache cache;
        private readonly Google.OrTools.ConstraintSolver.Solver solver;

        /// <summary>
        /// Initialize the all different constraint converter with a solver and or-tools cache.
        /// </summary>
        /// <param name="theSolver">Google or-tools solver instance.</param>
        /// <param name="theCache">Cache mapping between the model and Google or-tools solver.</param>
        public AllDifferentConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver, 
                                               OrToolsCache theCache)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            Contract.Requires<ArgumentNullException>(theCache != null);
            this.solver = theSolver;
            this.cache = theCache;
        }

        /// <summary>
        /// Map the all different constraint model into the or-tools solver.
        /// </summary>
        /// <param name="allDifferentConstraint">All different constraint model.</param>
        public void ProcessConstraint(AllDifferentConstraintModel allDifferentConstraint)
        {
            Contract.Requires<ArgumentNullException>(allDifferentConstraint != null);
            var theVector = this.cache.GetVectorByName(allDifferentConstraint.Expression.Text);
            var orAllDifferentConstraint = this.solver.MakeAllDifferent(theVector);
            this.solver.Add(orAllDifferentConstraint);
        }
    }
}
