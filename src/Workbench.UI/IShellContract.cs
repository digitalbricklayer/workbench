using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench
{
    /// <summary>
    /// Code contract for the <see cref="IShell"/> interface.
    /// </summary>
    [ContractClassFor(typeof(IShell))]
    internal abstract class IShellContract : IShell
    {
        private WorkspaceViewModel _workspace;
        private WorkspaceDocumentViewModel _currentDocument;

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

        public WorkspaceDocumentViewModel CurrentDocument
        {
            get
            {
                Contract.Ensures(Contract.Result<WorkspaceDocumentViewModel>() != null);
                return _currentDocument;
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _currentDocument = value;
            }
        }

        public void Close(CancelEventArgs cancelEventArgs)
        {
        }

        public void Close()
        {
        }
    }
}
