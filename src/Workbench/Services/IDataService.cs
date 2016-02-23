using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Services
{
    /// <summary>
    /// Contract for the data service.
    /// </summary>
    [ContractClass(typeof(IDataServiceContract))]
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

        /// <summary>
        /// Get the variable by the variable name.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <returns>Variable.</returns>
        VariableModel GetVariableByName(string variableName);
    }

    /// <summary>
    /// Code contract for the IDataService interface.
    /// </summary>
    [ContractClassFor(typeof(IDataService))]
    internal abstract class IDataServiceContract : IDataService
    {
        private IDataServiceContract()
        {
        }

        public WorkspaceModel Open(string file)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(file));
            Contract.Ensures(Contract.Result<WorkspaceModel>() != null);
            return default(WorkspaceModel);
        }

        public void Save(string file)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(file));
        }

        public WorkspaceModel GetWorkspace()
        {
            Contract.Ensures(Contract.Result<WorkspaceModel>() != null);
            return default(WorkspaceModel);
        }

        public VariableModel GetVariableByName(string variableName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(variableName));
            Contract.Ensures(Contract.Result<VariableModel>() != null);
            return default(VariableModel);
        }
    }
}
