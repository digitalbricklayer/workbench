namespace Workbench.ViewModels
{
    /// <summary>
    /// Interface to mark workspace tab view models.
    /// </summary>
    public interface IWorkspaceTabViewModel
    {
        /// <summary>
        /// Get whether the tab can be closed by the user.
        /// </summary>
        bool CloseTabIsVisible { get; }
    }
}
