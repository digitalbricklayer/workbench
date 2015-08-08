using AutoMapper;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    /// <summary>
    /// Profile for mapping view models to models.
    /// </summary>
    internal class ModelProfile : Profile
    {
        /// <summary>
        /// Configure the model profile.
        /// </summary>
        protected override void Configure()
        {
            CreateMap<WorkspaceViewModel, WorkspaceModel>();
            CreateMap<ModelViewModel, ModelModel>();
            CreateMap<SolutionViewModel, SolutionModel>();
            CreateMap<ConstraintViewModel, ConstraintModel>();
            CreateMap<VariableViewModel, VariableModel>();
            CreateMap<DomainViewModel, DomainModel>();
            CreateMap<ConnectionViewModel, ConnectionModel>();
            CreateMap<ConnectorViewModel, ConnectorModel>();
            CreateMap<ConstraintExpressionViewModel, ConstraintExpressionModel>();
            CreateMap<DomainExpressionViewModel, DomainExpressionModel>();
            CreateMap<ValueViewModel, ValueModel>();
        }
    }
}