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

        internal ModelMapper(VariableMapper theVariableMapper, DomainMapper theDomainMapper, ConstraintMapper theConstraintMapper)
        {
            this.variableMapper = theVariableMapper;
            this.domainMapper = theDomainMapper;
            this.constraintMapper = theConstraintMapper;
        }

        internal ModelMapper(ModelViewModelCache theCache)
        {
            this.variableMapper = new VariableMapper(theCache);
            this.domainMapper = new DomainMapper(theCache);
            this.constraintMapper = new ConstraintMapper(theCache);
        }

        internal ModelViewModel MapFrom(ModelModel theModelModel)
        {
            var modelViewModel = new ModelViewModel(theModelModel);

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