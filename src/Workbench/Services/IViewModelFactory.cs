using Workbench.ViewModels;

namespace Workbench.Services
{
    public interface IViewModelFactory
    {
        /// <summary>
        /// Create a new workspace view model.
        /// </summary>
        /// <returns>New workspace view model.</returns>
        WorkspaceViewModel CreateWorkspace();
    }
}