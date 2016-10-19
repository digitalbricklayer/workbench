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
}
