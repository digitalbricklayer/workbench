using AutoMapper;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ModelService
    {
        /// <summary>
        /// Intialize the model service with default values.
        /// </summary>
        internal ModelService()
        {
            Mapper.Initialize(configuration => configuration.AddProfile<ViewModelProfile>());
#if DEBUG
            Mapper.AssertConfigurationIsValid();
#endif
        }

        /// <summary>
        /// Map a workspace view model to a workspace model.
        /// </summary>
        /// <param name="theWorkspaceModel">Workspace model.</param>
        /// <returns>Workspace view model.</returns>
        internal WorkspaceViewModel MapFrom(WorkspaceModel theWorkspaceModel)
        {
            return Mapper.Map<WorkspaceViewModel>(theWorkspaceModel);
        }
    }
}
