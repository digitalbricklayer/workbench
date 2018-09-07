using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a model into a view model.
    /// </summary>
    public class WorkspaceMapper
    {
#if true
        private readonly DisplayMapper displayMapper;
#endif
        private readonly IViewModelFactory viewModelFactory;

        /// <summary>
        /// Initialize the model mapper with a window manager and view model factory.
        /// </summary>
        public WorkspaceMapper(IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);

            this.viewModelFactory = theViewModelFactory;
        }

        /// <summary>
        /// Map a workspace model to a workspace view model.
        /// </summary>
        /// <param name="theWorkspaceModel">Workspace model.</param>
        /// <returns>Workspace view model.</returns>
        public WorkspaceViewModel MapFrom(WorkspaceModel theWorkspaceModel)
        {
            var workspaceViewModel = this.viewModelFactory.CreateWorkspace();
#if true
            workspaceViewModel.ModelEditor = this.displayMapper.MapFrom(theWorkspaceModel);
#endif
            return workspaceViewModel;
        }
    }
}
