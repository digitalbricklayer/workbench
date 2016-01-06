using System;
using System.Diagnostics;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Service for controlling access to the model.
    /// </summary>
    public class DataService : IDataService
    {
        private readonly IWorkspaceReaderWriter readerWriter;
        private WorkspaceModel currentWorkspace;

        /// <summary>
        /// Initialize a new data service with a workspace reader/writer.
        /// </summary>
        /// <param name="theReaderWriter">Persistence reader/writer.</param>
        public DataService(IWorkspaceReaderWriter theReaderWriter)
        {
            if (theReaderWriter == null)
                throw new ArgumentNullException("theReaderWriter");
            this.readerWriter = theReaderWriter;
            this.currentWorkspace = this.GetWorkspace();
        }

        /// <summary>
        /// Open the file and read the contents.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        public WorkspaceModel Open(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("file");
            this.currentWorkspace = this.readerWriter.Read(file);

            return this.currentWorkspace;
        }

        /// <summary>
        /// Save the current workspace to a file.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        public void Save(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("file");
            Debug.Assert(this.currentWorkspace != null);
            this.readerWriter.Write(file, this.currentWorkspace);
        }

        /// <summary>
        /// Get the current workspace.
        /// </summary>
        /// <returns>Current workspace.</returns>
        public WorkspaceModel GetWorkspace()
        {
            if (this.currentWorkspace != null)
                return this.currentWorkspace;
            return new WorkspaceModel();
        }
    }
}
