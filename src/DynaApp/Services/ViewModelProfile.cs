using AutoMapper;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    /// <summary>
    /// Profile for mapping models to view models.
    /// </summary>
    internal class ViewModelProfile : Profile
    {
        /// <summary>
        /// Configure the profile.
        /// </summary>
        protected override void Configure()
        {
            CreateMap<WorkspaceModel, WorkspaceViewModel>();
            CreateMap<ModelModel, ModelViewModel>();
            CreateMap<SolutionModel, SolutionViewModel>();
            CreateMap<ConstraintModel, ConstraintViewModel>();
            CreateMap<VariableModel, VariableViewModel>();
            CreateMap<DomainModel, DomainViewModel>();
            CreateMap<ConnectionModel, ConnectionViewModel>();
            CreateMap<ConnectorModel, ConnectorViewModel>();
            CreateMap<ConstraintExpressionModel, ConstraintExpressionViewModel>();
            CreateMap<ValueModel, ValueViewModel>();
        }
    }
}
