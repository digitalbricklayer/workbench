using System;
using System.Diagnostics.Contracts;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Code contract for the <see cref="Workbench.ViewModels.IShell"/> interface.
    /// </summary>
    [ContractClassFor(typeof(IShell))]
    internal abstract class IShellContract : IShell
    {
        private WorkspaceViewModel _workspace;

        public WorkspaceViewModel Workspace
        {
            get
            {
                Contract.Ensures(Contract.Result<WorkspaceViewModel>() != null);
                return this._workspace;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this._workspace = value;
            }
        }
    }
}
