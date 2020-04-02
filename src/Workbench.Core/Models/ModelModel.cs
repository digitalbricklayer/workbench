using System;
using Workbench.Core.Solvers;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A model for specifying the problem.
    /// <remarks>Just a very simple finite integer domain at the moment.</remarks>
    /// </summary>
    [Serializable]
    public sealed class ModelModel : BundleModel
    {
        /// <summary>
        /// Initialize a model with a name.
        /// </summary>
        /// <param name="theName">Model name.</param>
        /// <param name="workspace">Workspace the model is a part.</param>
        public ModelModel(ModelName theName, WorkspaceModel workspace)
            : base(theName)
        {
            Workspace = workspace;
        }

        /// <summary>
        /// Initialize a model with default values.
        /// </summary>
        public ModelModel(WorkspaceModel workspace)
        {
            Workspace = workspace;
        }

        /// <summary>
        /// Solve the model.
        /// </summary>
        /// <returns>Solve result.</returns>
        public SolveResult Solve()
        {
            var solver = CreateSolver();

            try
            {
                return solver.Solve(this);
            }
            finally
            {
                if (solver is IDisposable disposableSolver)
                {
                    disposableSolver.Dispose();
                }
            }
        }

        private ISolvable CreateSolver()
        {
            return new OrToolsSolver();
        }
    }
}
