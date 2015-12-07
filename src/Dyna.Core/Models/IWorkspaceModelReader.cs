namespace Dyna.Core.Models
{
    public interface IWorkspaceModelReader
    {
        /// <summary>
        /// Read a workspace model from a file.
        /// </summary>
        /// <returns>Workspace model.</returns>
        WorkspaceModel Read(string filename);
    }
}