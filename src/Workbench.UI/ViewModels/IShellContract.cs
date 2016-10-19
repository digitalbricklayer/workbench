using System;
using System.Diagnostics.Contracts;

namespace Workbench.ViewModels
{
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