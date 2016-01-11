using Workbench.ViewModels;

namespace Workbench
{
    public interface IShell
    {
        /// <summary>
        /// Gets or sets the workspace view model.
        /// </summary>
        WorkspaceViewModel Workspace { get; set; }
    }
}
