using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Reads or writes the workspace.
    /// </summary>
    public sealed class WorkspaceReaderWriter : IWorkspaceReaderWriter
    {
        private readonly IWorkspaceReader _reader;
        private readonly IWorkspaceWriter _writer;

        public WorkspaceReaderWriter(IWorkspaceReader theReader, IWorkspaceWriter theWriter)
        {
            _reader = theReader;
            _writer = theWriter;
        }

        /// <summary>
        /// Read a workspace model from a file.
        /// </summary>
        /// <returns>Workspace model.</returns>
        public WorkspaceModel Read(string filename)
        {
            return _reader.Read(filename);
        }

        /// <summary>
        /// Write a workspace model to a file.
        /// </summary>
        /// <param name="filename">File Path.</param>
        /// <param name="theWorkspace">Workspace model.</param>
        public void Write(string filename, WorkspaceModel theWorkspace)
        {
            _writer.Write(filename, theWorkspace);
        }
    }
}
