namespace Workbench.ViewModels
{
    /// <summary>
    /// Interface to mark tab view models.
    /// </summary>
    public interface ITabViewModel
    {
        /// <summary>
        /// Get whether the tab can be closed by the user.
        /// </summary>
        bool CloseTabIsVisible { get; }

        /// <summary>
        /// Close the tab initiated by the user.
        /// </summary>
        void CloseTab();
    }
}
