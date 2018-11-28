using System.ComponentModel;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench
{
    [ContractClass(typeof(IShellContract))]
    public interface IShell
    {
        /// <summary>
        /// Gets or sets the work area.
        /// </summary>
        WorkspaceViewModel Workspace { get; set; }

        /// <summary>
        /// Gets or sets the current workspace document.
        /// </summary>
        WorkspaceDocumentViewModel CurrentDocument { get; set; }

        /// <summary>
        /// Close the shell with the option to cancel.
        /// </summary>
        void OnClose(CancelEventArgs cancelEventArgs);

        /// <summary>
        /// Close the shell.
        /// </summary>
        void Close();

        /// <summary>
        /// Open the document.
        /// </summary>
        /// <param name="theDocument">Workspace document.</param>
        void OpenDocument(WorkspaceDocumentViewModel theDocument);
    }
}
