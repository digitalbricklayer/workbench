using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace Workbench.ViewModels
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
        void Close(CancelEventArgs cancelEventArgs);

        /// <summary>
        /// Close the shell.
        /// </summary>
        void Close();
    }
}
