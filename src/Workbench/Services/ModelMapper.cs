using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a model model into a view model.
    /// </summary>
    public class ModelMapper
    {
        private readonly VariableMapper variableMapper;
        private readonly DomainMapper domainMapper;
        private readonly ConstraintMapper constraintMapper;
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        public ModelMapper(VariableMapper theVariableMapper,
                             ConstraintMapper theConstraintMapper,
                             DomainMapper theDomainMapper,
                             IWindowManager theWindowManager,
                             IEventAggregator theEventAggregator)
        {
            this.variableMapper = theVariableMapper;
            this.domainMapper = theDomainMapper;
            this.constraintMapper = theConstraintMapper;
            this.windowManager = theWindowManager;
            this.eventAggregator = theEventAggregator;
        }

        internal ModelViewModel MapFrom(ModelModel theModelModel)
        {
            var modelViewModel = new ModelViewModel(theModelModel,
                                                    this.windowManager,
                                                    this.eventAggregator);

            foreach (var domainModel in theModelModel.Domains)
            {
                var domainViewModel = this.domainMapper.MapFrom(domainModel);
                modelViewModel.FixupDomain(domainViewModel);
            }

            foreach (var constraintModel in theModelModel.Constraints)
            {
                var constraintViewModel = this.constraintMapper.MapFrom(constraintModel);
                modelViewModel.FixupConstraint(constraintViewModel);
            }

            foreach (var variableModel in theModelModel.Singletons)
            {
                var variableViewModel = this.variableMapper.MapFrom(variableModel);
                modelViewModel.FixupSingletonVariable(variableViewModel);
            }

            foreach (var aggregateModel in theModelModel.Aggregates)
            {
                var variableViewModel = this.variableMapper.MapFrom(aggregateModel);
                modelViewModel.FixupAggregateVariable(variableViewModel);
            }

            return modelViewModel;
        }
    }
}