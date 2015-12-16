using Dyna.Core.Models;

namespace DynaApp.Services
{
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
        /// <param name="theWorkspace">Workspace to save to disk.</param>
        void Save(string file, WorkspaceModel theWorkspace);
    }
}