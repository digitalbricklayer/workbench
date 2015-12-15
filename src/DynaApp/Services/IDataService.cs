using Dyna.Core.Models;

namespace DynaApp.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Get a new workspace.
        /// </summary>
        /// <returns>A new workspace.</returns>
        WorkspaceModel GetWorkspace();

        /// <summary>
        /// Get a model for the workspace.
        /// </summary>
        /// <returns>A new model.</returns>
        ModelModel GetModelFor(WorkspaceModel theWorkspace);
    }
}