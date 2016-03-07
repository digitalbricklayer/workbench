using System;
using System.Diagnostics.Contracts;

namespace Workbench.ViewModels
{
    [ContractClass(typeof(IShellContract))]
    public interface IShell
    {
        /// <summary>
        /// Gets or sets the workspace view model.
        /// </summary>
        WorkspaceViewModel Workspace { get; set; }
    }

    /// <summary>
    /// Code contract for the IShell interface.
    /// </summary>
    [ContractClassFor(typeof(IShell))]
    internal abstract class IShellContract : IShell
    {
        private WorkspaceViewModel workspace;

        public WorkspaceViewModel Workspace
        {
            get
            {
                Contract.Ensures(Contract.Result<WorkspaceViewModel>() != null);
                return workspace;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                workspace = value;
            }
        }
    }
}
