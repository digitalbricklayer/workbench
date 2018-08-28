namespace Workbench.ViewModels
{
    /// <summary>
    /// Interface to mark workspace tab view models.
    /// </summary>
    public interface IWorkspaceTabViewModel
    {
        /// <summary>
        /// Gets whether the tab can be closed by the user.
        /// </summary>
        bool CloseTabIsVisible { get; }

        /// <summary>
        /// Gets or sets the tab text.
        /// </summary>
        string Text { get; set; }
    }
}
