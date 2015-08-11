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
        /// Gets the profile name.
        /// </summary>
        public override string ProfileName
        {
            get
            {
                return "View Model Profile";
            }
        }

        /// <summary>
        /// Configure the profile.
        /// </summary>
        protected override void Configure()
        {
            CreateMap<WorkspaceModel, WorkspaceViewModel>()
                .ForMember(_ => _.IsDirty, options => options.Ignore())
                .ForMember(_ => _.WorkspaceModel, options => options.Ignore())
                .ForMember(_ => _.SelectedDisplayMode, options => options.Ignore())
                .ForMember(_ => _.AvailableDisplayModes, options => options.Ignore())
                .ForMember(_ => _.SelectedDisplayViewModel, options => options.Ignore());
            CreateMap<ModelModel, ModelViewModel>()
                .ForMember(_ => _.Graphics, options => options.Ignore());
            CreateMap<SolutionModel, SolutionViewModel>();
            CreateMap<GraphicModel, GraphicViewModel>()
                .ForMember(_ => _.IsSelected, options => options.Ignore())
                .Include<ConstraintModel, ConstraintViewModel>()
                .Include<VariableModel, VariableViewModel>()
                .Include<DomainModel, DomainViewModel>();
            CreateMap<ConstraintModel, ConstraintViewModel>();
            CreateMap<VariableModel, VariableViewModel>();
            CreateMap<DomainModel, DomainViewModel>();
            CreateMap<ConnectionModel, ConnectionViewModel>()
                .ForMember(_ => _.Points, options => options.Ignore());
            CreateMap<ConnectorModel, ConnectorViewModel>();
            CreateMap<ConstraintExpressionModel, ConstraintExpressionViewModel>();
            CreateMap<DomainExpressionModel, DomainExpressionViewModel>();
            CreateMap<ValueModel, ValueViewModel>();
        }
    }
}
