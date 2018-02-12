using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(theReaderWriter != null);

            this.readerWriter = theReaderWriter;
            this.currentWorkspace = this.GetWorkspace();
        }

        /// <summary>
        /// Open the file and read the contents.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        public WorkspaceModel Open(string file)
        {
            Contract.Ensures(Contract.Result<WorkspaceModel>() != null);

            this.currentWorkspace = this.readerWriter.Read(file);
            return this.currentWorkspace;
        }

        /// <summary>
        /// Save the current workspace to a file.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        public void Save(string file)
        {
            Contract.Assume(this.currentWorkspace != null);

            this.readerWriter.Write(file, this.currentWorkspace);
        }

        /// <summary>
        /// Get the current workspace.
        /// </summary>
        /// <returns>Current workspace.</returns>
        public WorkspaceModel GetWorkspace()
        {
            return this.currentWorkspace ?? new WorkspaceModel();
        }

        /// <summary>
        /// Get the variable by the variable name.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <returns>Variable.</returns>
        public VariableGraphicModel GetVariableByName(string variableName)
        {
            return this.currentWorkspace.Model.GetVariableByName(variableName);
        }
    }
}
