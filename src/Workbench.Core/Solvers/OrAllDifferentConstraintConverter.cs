using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Convert the all different constraint model representation into a representation usable 
    /// by the or-tools solver.
    /// </summary>
    internal sealed class OrAllDifferentConstraintConverter
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
        internal OrAllDifferentConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache, ModelModel theModel)
        {
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
            var theVector = this.cache.GetVectorByName(allDifferentConstraint.Expression.Text);
            var orAllDifferentConstraint = this.solver.MakeAllDifferent(theVector);
            this.solver.Add(orAllDifferentConstraint);
        }
    }
}
