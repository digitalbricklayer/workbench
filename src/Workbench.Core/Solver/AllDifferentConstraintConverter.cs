using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Convert the all different model representation into a representation usable 
    /// by the or-tools solver.
    /// </summary>
    class AllDifferentConstraintConverter
    {
        private readonly OrToolsCache cache;
        private readonly Google.OrTools.ConstraintSolver.Solver solver;

        public AllDifferentConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver, 
                                               OrToolsCache theCache)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            Contract.Requires<ArgumentNullException>(theCache != null);
            this.solver = theSolver;
            this.cache = theCache;
        }

        public void ProcessConstraint(AllDifferentConstraintModel allDifferentConstraint)
        {
            Contract.Requires<ArgumentNullException>(allDifferentConstraint != null);
            var theVector = this.cache.GetVectorByName(allDifferentConstraint.Expression.Text);
            var orAllDifferentConstraint = this.solver.MakeAllDifferent(theVector);
            this.solver.Add(orAllDifferentConstraint);
        }
    }
}
