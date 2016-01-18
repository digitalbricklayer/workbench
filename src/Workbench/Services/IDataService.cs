using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Contract for the data service.
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Open the file and read the contents.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        WorkspaceModel Open(string file);

        /// <summary>
        /// Save the workspace to a file.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        void Save(string file);

        /// <summary>
        /// Get the current workspace.
        /// </summary>
        /// <returns>Current workspace.</returns>
        WorkspaceModel GetWorkspace();
    }
}