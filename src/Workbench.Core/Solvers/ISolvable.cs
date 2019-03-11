using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Interface for all solvers.
    /// </summary>
    public interface ISolvable
    {
        SolveResult Solve(ModelModel theModel);
    }
}