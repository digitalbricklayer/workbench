using DynaApp.Models;
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

        /// <summary>
        /// Initialize the model mapper with default values.
        /// </summary>
        internal WorkspaceMapper(ModelViewModelCache theCache)
        {
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
            var workspaceViewModel = new WorkspaceViewModel
            {
                WorkspaceModel = theWorkspaceModel,
                Model = this.modelMapper.MapFrom(theWorkspaceModel.Model),
                Solution = this.solutionMapper.MapFrom(theWorkspaceModel.Solution)
            };

            return workspaceViewModel;
        }
    }
}
