using AutoMapper;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ModelService
    {
        /// <summary>
        /// Map a workspace view model to a workspace model.
        /// </summary>
        /// <param name="theWorkspaceViewModel">Workspace view model.</param>
        /// <returns>Workspace model.</returns>
        internal WorkspaceModel MapFrom(WorkspaceViewModel theWorkspaceViewModel)
        {
            Mapper.Initialize(configuration => configuration.AddProfile<ModelProfile>());
            return Mapper.Map<WorkspaceModel>(theWorkspaceViewModel);
        }

        /// <summary>
        /// Map a workspace view model to a workspace model.
        /// </summary>
        /// <param name="theWorkspaceModel">Workspace model.</param>
        /// <returns>Workspace view model.</returns>
        internal WorkspaceViewModel MapFrom(WorkspaceModel theWorkspaceModel)
        {
            Mapper.Initialize(configuration => configuration.AddProfile<ViewModelProfile>());
            return Mapper.Map<WorkspaceViewModel>(theWorkspaceModel);
        }
    }
}
