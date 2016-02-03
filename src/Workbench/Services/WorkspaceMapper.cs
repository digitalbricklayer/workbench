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
        private readonly DisplayMapper displayMapper;
        private readonly IWindowManager windowManager;
        private readonly IViewModelFactory viewModelFactory;
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initialize the model mapper with a window manager and view model factory.
        /// </summary>
        public WorkspaceMapper(IWindowManager theWindowManager, 
                               IViewModelFactory theViewModelFactory, 
                               IEventAggregator theEventAggregator)
        {
            if (theWindowManager == null)
                throw new ArgumentNullException("theWindowManager");

            if (theViewModelFactory == null)
                throw new ArgumentNullException("theViewModelFactory");

            if (theEventAggregator == null)
                throw new ArgumentNullException("theEventAggregator");

            var theCache = new ModelViewModelCache();
            this.windowManager = theWindowManager;
            this.viewModelFactory = theViewModelFactory;
            this.eventAggregator = theEventAggregator;
            this.modelMapper = new ModelMapper(theCache,
                                               this.windowManager,
                                               this.eventAggregator);
            this.displayMapper = new DisplayMapper();
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
            workspaceViewModel.Viewer = this.solutionMapper.MapFrom(theWorkspaceModel.Solution);
            workspaceViewModel.Designer = this.displayMapper.MapFrom(theWorkspaceModel.Display);

            return workspaceViewModel;
        }
    }
}
