using System;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a model model into a view model.
    /// </summary>
    internal class ModelMapper
    {
        private readonly VariableMapper variableMapper;
        private readonly DomainMapper domainMapper;
        private readonly ConstraintMapper constraintMapper;
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        internal ModelMapper(ViewModelCache theCache, IWindowManager theWindowManager, IEventAggregator theEventAggregator)
        {
            if (theWindowManager == null)
                throw new ArgumentNullException("theWindowManager");

            if (theEventAggregator == null)
                throw new ArgumentNullException("theEventAggregator");

            this.windowManager = theWindowManager;
            this.eventAggregator = theEventAggregator;
            this.variableMapper = new VariableMapper(theCache);
            this.domainMapper = new DomainMapper(theCache);
            this.constraintMapper = new ConstraintMapper(theCache);
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

            foreach (var variableModel in theModelModel.Variables)
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