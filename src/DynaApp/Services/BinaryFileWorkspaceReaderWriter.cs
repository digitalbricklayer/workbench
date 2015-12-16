using System;
using Dyna.Core.Models;

namespace DynaApp.Services
{
    /// <summary>
    /// Reads/writes the workspace to a binary file.
    /// </summary>
    public sealed class BinaryFileWorkspaceReaderWriter : IWorkspaceReaderWriter
    {
        private readonly IWorkspaceReader reader;
        private readonly IWorkspaceWriter writer;

        public BinaryFileWorkspaceReaderWriter(IWorkspaceReader theReader,
                                               IWorkspaceWriter theWriter)
        {
            if (theReader == null)
                throw new ArgumentNullException("theReader");
            if (theWriter == null)
                throw new ArgumentNullException("theWriter");
            this.reader = theReader;
            this.writer = theWriter;
        }

        /// <summary>
        /// Read a workspace model from a file.
        /// </summary>
        /// <returns>Workspace model.</returns>
        public WorkspaceModel Read(string filename)
        {
            return this.reader.Read(filename);
        }

        /// <summary>
        /// Write a workspace model to a file.
        /// </summary>
        /// <param name="filename">File Path.</param>
        /// <param name="theWorkspace">Workspace model.</param>
        public void Write(string filename, WorkspaceModel theWorkspace)
        {
            this.writer.Write(filename, theWorkspace);
        }
    }
}
