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
        private readonly ModelMapper modelMapper;
        private readonly SolutionMapper solutionMapper;
        private readonly DisplayMapper displayMapper;
        private readonly IViewModelFactory viewModelFactory;

        /// <summary>
        /// Initialize the model mapper with a window manager and view model factory.
        /// </summary>
        public WorkspaceMapper(ModelMapper theModelMapper,
                               SolutionMapper theSolutionMapper,
                               DisplayMapper theDisplayMapper,
                               IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theModelMapper != null);
            Contract.Requires<ArgumentNullException>(theSolutionMapper != null);
            Contract.Requires<ArgumentNullException>(theDisplayMapper != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);

            this.modelMapper = theModelMapper;
            this.displayMapper = theDisplayMapper;
            this.solutionMapper = theSolutionMapper;
            this.viewModelFactory = theViewModelFactory;
        }

        /// <summary>
        /// Map a workspace model to a workspace view model.
        /// </summary>
        /// <param name="theWorkspaceModel">Workspace model.</param>
        /// <returns>Workspace view model.</returns>
        public WorkAreaViewModel MapFrom(WorkspaceModel theWorkspaceModel)
        {
            var workspaceViewModel = this.viewModelFactory.CreateWorkspace();
            workspaceViewModel.Model = this.modelMapper.MapFrom(theWorkspaceModel.Model);
            workspaceViewModel.Solution = new SolutionViewModel(workspaceViewModel,
                                                                this.displayMapper.MapFrom(theWorkspaceModel.Solution.Display),
                                                                this.solutionMapper.MapFrom(theWorkspaceModel.Solution));

            return workspaceViewModel;
        }
    }
}
