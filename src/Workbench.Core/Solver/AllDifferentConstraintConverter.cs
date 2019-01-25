using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Convert the all different constraint model representation into a representation usable 
    /// by the or-tools solver.
    /// </summary>
    internal class AllDifferentConstraintConverter
    {
        private readonly OrToolsCache cache;
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private readonly ModelModel model;

        /// <summary>
        /// Initialize the all different constraint converter with a solver and or-tools cache.
        /// </summary>
        /// <param name="theSolver">Google or-tools solver instance.</param>
        /// <param name="theCache">Cache mapping between the model and Google or-tools solver.</param>
        /// <param name="theModel">Model.</param>
        internal AllDifferentConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            Contract.Requires<ArgumentNullException>(theCache != null);
            this.solver = theSolver;
            this.cache = theCache;
            this.model = theModel;
        }

        /// <summary>
        /// Map the all different constraint model into the or-tools solver.
        /// </summary>
        /// <param name="allDifferentConstraint">All different constraint model.</param>
        internal void ProcessConstraint(AllDifferentConstraintModel allDifferentConstraint)
        {
            Contract.Requires<ArgumentNullException>(allDifferentConstraint != null);
            var variableToConstrain = this.cache.GetVectorByName(allDifferentConstraint.Expression.Text);
            var orAllDifferentConstraint = this.solver.MakeAllDifferent(variableToConstrain);
            this.solver.Add(orAllDifferentConstraint);
        }
    }
}
