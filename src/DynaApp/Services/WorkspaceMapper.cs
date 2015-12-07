using System;
using Dyna.Core.Models;
using DynaApp.Factories;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    /// <summary>
    /// Maps a model into a view model.
    /// </summary>
    internal class WorkspaceMapper
    {
        private readonly ModelMapper modelMapper;
        private readonly SolutionMapper solutionMapper;
        private readonly IViewModelFactory viewModelFactory;

        /// <summary>
        /// Initialize the model mapper with default values.
        /// </summary>
        internal WorkspaceMapper(ModelViewModelCache theCache, IViewModelFactory theViewModelFactory)
        {
            if (theViewModelFactory == null)
                throw new ArgumentNullException("theViewModelFactory");
            this.viewModelFactory = theViewModelFactory;
            this.modelMapper = new ModelMapper(theCache);
            this.solutionMapper = new SolutionMapper(theCache);
        }

        /// <summary>
        /// Map a workspace model to a workspace view model.
        /// </summary>
        /// <param name="theWorkspaceModel">Workspace model.</param>
        /// <returns>Workspace view model.</returns>
        internal WorkspaceViewModel MapFrom(WorkspaceModel theWorkspaceModel)
        {
            var workspaceViewModel = this.viewModelFactory.CreateWorkspace();
            workspaceViewModel.WorkspaceModel = theWorkspaceModel;
            workspaceViewModel.Model = this.modelMapper.MapFrom(theWorkspaceModel.Model);
            workspaceViewModel.Solution = this.solutionMapper.MapFrom(theWorkspaceModel.Solution);

            return workspaceViewModel;
        }
    }
}
