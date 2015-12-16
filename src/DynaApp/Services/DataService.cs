using System;
using Dyna.Core.Models;

namespace DynaApp.Services
{
    /// <summary>
    /// Service for controlling access to the model.
    /// </summary>
    public class DataService : IDataService
    {
        private readonly IWorkspaceReaderWriter readerWriter;

        public DataService(IWorkspaceReaderWriter theReaderWriter)
        {
            if (theReaderWriter == null)
                throw new ArgumentNullException("theReaderWriter");
            this.readerWriter = theReaderWriter;
        }

        /// <summary>
        /// Open the file and read the contents.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        public WorkspaceModel Open(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("file");
            return this.readerWriter.Read(file);
        }

        /// <summary>
        /// Save the workspace to a file.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        /// <param name="theWorkspace">Workspace to save to disk.</param>
        public void Save(string file, WorkspaceModel theWorkspace)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("file");
            this.readerWriter.Write(file, theWorkspace);
        }
    }
}
