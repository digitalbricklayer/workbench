using Caliburn.Micro;
using DynaApp.ViewModels;

namespace DynaApp.Factories
{
    /// <summary>
    /// View model factory implemented using the Caliburn Micro simple container.
    /// </summary>
    public sealed class SimpleContainerViewModelFactory : IViewModelFactory
    {
        /// <summary>
        /// Create a workspace view model.
        /// </summary>
        /// <returns>Workspace view model.</returns>
        public WorkspaceViewModel CreateWorkspace()
        {
            return IoC.Get<WorkspaceViewModel>();
        }
    }
}
