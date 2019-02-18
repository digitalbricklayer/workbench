using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Implementation of the solver using the AC-1 algorithm.
    /// </summary>
    public class Ac1Solver : ISolvable
    {
        public SolveResult Solve(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            if (!new ModelValidator(theModel).Validate()) return SolveResult.InvalidModel;
			
			return SolveResult.Failed;
        }
    }
}
