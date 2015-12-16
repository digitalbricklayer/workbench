using System;
using Caliburn.Micro;
using Dyna.Core.Models;
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
        private readonly IWindowManager windowManager;

        /// <summary>
        /// Initialize the model mapper with a model view model cache.
        /// </summary>
        internal WorkspaceMapper(ModelViewModelCache theCache, IWindowManager theWindowManager)
        {
            if (theCache == null)
                throw new ArgumentNullException("theCache");

            if (theWindowManager == null)
                throw new ArgumentNullException("theWindowManager");

            this.windowManager = theWindowManager;
            this.modelMapper = new ModelMapper(theCache, this.windowManager);
            this.solutionMapper = new SolutionMapper(theCache);
        }

        /// <summary>
        /// Map a workspace model to a workspace view model.
        /// </summary>
        /// <param name="theWorkspaceModel">Workspace model.</param>
        /// <returns>Workspace view model.</returns>
        internal WorkspaceViewModel MapFrom(WorkspaceModel theWorkspaceModel)
        {
            var workspaceViewModel = new WorkspaceViewModel(theWorkspaceModel, this.windowManager);
            workspaceViewModel.Model = this.modelMapper.MapFrom(theWorkspaceModel.Model);
            workspaceViewModel.Solution = this.solutionMapper.MapFrom(theWorkspaceModel.Solution);

            return workspaceViewModel;
        }
    }
}
