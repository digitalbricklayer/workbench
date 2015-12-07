using DynaApp.Factories;
using DynaApp.Services;
using DynaApp.ViewModels;

namespace Dyna.UI.Tests.Unit
{
    /// <summary>
    /// View model factory used whilst testing.
    /// </summary>
    public class SimpleViewModelFactory : IViewModelFactory
    {
        private readonly DataService dataService;

        public SimpleViewModelFactory(DataService theDataService)
        {
            this.dataService = theDataService;
        }

        /// <summary>
        /// Create a workspace view model.
        /// </summary>
        /// <returns>Workspace view model.</returns>
        public WorkspaceViewModel CreateWorkspace()
        {
            return new WorkspaceViewModel(this.dataService);
        }
    }
}