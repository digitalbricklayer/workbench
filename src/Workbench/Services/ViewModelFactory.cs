using Caliburn.Micro;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Factory for creating view models.
    /// </summary>
    public class ViewModelFactory : IViewModelFactory
    {
        /// <summary>
        /// Create a new workspace view model.
        /// </summary>
        /// <returns>New workspace view model.</returns>
        public WorkspaceViewModel CreateWorkspace()
        {
            return IoC.Get<WorkspaceViewModel>();
        }
    }
}
