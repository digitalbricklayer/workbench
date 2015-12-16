using System;
using Caliburn.Micro;
using Dyna.Core.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
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

        internal ModelMapper(ModelViewModelCache theCache, IWindowManager theWindowManager)
        {
            if (theWindowManager == null)
                throw new ArgumentNullException("theWindowManager");

            this.windowManager = theWindowManager;
            this.variableMapper = new VariableMapper(theCache);
            this.domainMapper = new DomainMapper(theCache);
            this.constraintMapper = new ConstraintMapper(theCache);
        }

        internal ModelViewModel MapFrom(ModelModel theModelModel)
        {
            var modelViewModel = new ModelViewModel(theModelModel, this.windowManager);

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

            return modelViewModel;
        }
    }
}