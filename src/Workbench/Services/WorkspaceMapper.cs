using System;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a model into a view model.
    /// </summary>
    public class WorkspaceMapper
    {
        private readonly ModelMapper modelMapper;
        private readonly SolutionMapper solutionMapper;
        private readonly IWindowManager windowManager;
        private readonly IViewModelFactory viewModelFactory;

        /// <summary>
        /// Initialize the model mapper with a model view model cache.
        /// </summary>
        public WorkspaceMapper(IWindowManager theWindowManager, IViewModelFactory theViewModelFactory)
        {
            if (theWindowManager == null)
                throw new ArgumentNullException("theWindowManager");

            if (theViewModelFactory == null)
                throw new ArgumentNullException("theViewModelFactory");

            var theCache = new ModelViewModelCache();
            this.windowManager = theWindowManager;
            this.viewModelFactory = theViewModelFactory;
            this.modelMapper = new ModelMapper(theCache, this.windowManager);
            this.solutionMapper = new SolutionMapper(theCache);
        }

        /// <summary>
        /// Map a workspace model to a workspace view model.
        /// </summary>
        /// <param name="theWorkspaceModel">Workspace model.</param>
        /// <returns>Workspace view model.</returns>
        public WorkspaceViewModel MapFrom(WorkspaceModel theWorkspaceModel)
        {
            var workspaceViewModel = this.viewModelFactory.CreateWorkspace();
            workspaceViewModel.Model = this.modelMapper.MapFrom(theWorkspaceModel.Model);
            workspaceViewModel.Solution = this.solutionMapper.MapFrom(theWorkspaceModel.Solution);

            return workspaceViewModel;
        }
    }
}
