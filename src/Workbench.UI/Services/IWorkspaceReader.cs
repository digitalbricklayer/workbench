using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Services
{
    [ContractClass(typeof(IWorkspaceReaderContract))]
    public interface IWorkspaceReader
    {
        /// <summary>
        /// Read a workspace model from a file.
        /// </summary>
        /// <returns>Workspace model.</returns>
        WorkspaceModel Read(string filename);
    }

    /// <summary>
    /// Code contract for the IWorkspaceReader interface.
    /// </summary>
    [ContractClassFor(typeof(IWorkspaceReader))]
    internal abstract class IWorkspaceReaderContract : IWorkspaceReader
    {
        public WorkspaceModel Read(string filename)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(filename));
            Contract.Ensures(Contract.Result<WorkspaceModel>() != null);

            return default(WorkspaceModel);
        }
    }
}
