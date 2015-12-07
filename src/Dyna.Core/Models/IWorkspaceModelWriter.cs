namespace Dyna.Core.Models
{
    public interface IWorkspaceModelWriter
    {
        /// <summary>
        /// Write a workspace model to a file.
        /// </summary>
        /// <param name="filename">File path.</param>
        /// <param name="theWorkspace">Workspace model.</param>
        void Write(string filename, WorkspaceModel theWorkspace);
    }
}