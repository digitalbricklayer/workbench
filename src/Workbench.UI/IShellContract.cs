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
        private IWorkspace _workspace;
        private IWorkspaceDocument _currentDocument;

        public IWorkspace Workspace
        {
            get
            {
                Contract.Ensures(Contract.Result<IWorkspace>() != null);
                return _workspace;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _workspace = value;
            }
        }

        public IWorkspaceDocument CurrentDocument
        {
            get
            {
                Contract.Ensures(Contract.Result<IWorkspaceDocument>() != null);
                return _currentDocument;
            }

            set
            {
                _currentDocument = value;
            }
        }

        public void OnClose(CancelEventArgs cancelEventArgs)
        {
        }

        public void Close()
        {
        }

        public void OpenDocument(IWorkspaceDocument theDocument)
        {
            Contract.Requires<ArgumentNullException>(theDocument != null);
        }

        public bool CloseDocument()
        {
            return default(bool);
        }
    }
}
