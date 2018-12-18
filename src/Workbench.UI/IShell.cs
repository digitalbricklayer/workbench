using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace Workbench
{
    [ContractClass(typeof(IShellContract))]
    public interface IShell
    {
        /// <summary>
        /// Gets or sets the work area.
        /// </summary>
        IWorkspace Workspace { get; set; }

        /// <summary>
        /// Gets or sets the current workspace document.
        /// </summary>
        IWorkspaceDocument CurrentDocument { get; set; }

        /// <summary>
        /// Close the shell with the option to cancel.
        /// </summary>
        void OnClose(CancelEventArgs cancelEventArgs);

        /// <summary>
        /// Close the shell.
        /// </summary>
        void Close();

        /// <summary>
        /// Open the workspace document.
        /// </summary>
        /// <param name="theDocument">Workspace document.</param>
        /// <returns>True if the document was successfully saved, false if the user cancelled the operation.</returns>
        void OpenDocument(IWorkspaceDocument theDocument);

        /// <summary>
        /// Close the current document.
        /// </summary>
        /// <returns></returns>
        /// <returns>True if the document was successfully saved, false if the user cancelled the operation.</returns>
        bool CloseDocument();
    }
}
