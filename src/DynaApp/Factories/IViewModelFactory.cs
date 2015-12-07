using DynaApp.ViewModels;

namespace DynaApp.Factories
{
    /// <summary>
    /// Contract for a view model factory.
    /// </summary>
    public interface IViewModelFactory
    {
        /// <summary>
        /// Create a workspace view model.
        /// </summary>
        /// <returns>Workspace view model.</returns>
        WorkspaceViewModel CreateWorkspace();
    }
}
