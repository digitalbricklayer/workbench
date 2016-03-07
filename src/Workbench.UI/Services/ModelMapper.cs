using System;
using System.Diagnostics.Contracts;
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
        private readonly IViewModelFactory viewModelFactory;

        public ModelMapper(VariableMapper theVariableMapper,
                           ConstraintMapper theConstraintMapper,
                           DomainMapper theDomainMapper,
                           IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theVariableMapper != null);
            Contract.Requires<ArgumentNullException>(theConstraintMapper != null);
            Contract.Requires<ArgumentNullException>(theDomainMapper != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);

            this.variableMapper = theVariableMapper;
            this.domainMapper = theDomainMapper;
            this.constraintMapper = theConstraintMapper;
            this.viewModelFactory = theViewModelFactory;
        }

        internal ModelViewModel MapFrom(ModelModel theModelModel)
        {
            var modelViewModel = this.viewModelFactory.CreateModel(theModelModel);

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