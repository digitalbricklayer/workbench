using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ModelMapper
    {
        private readonly VariableMapper variableMapper;
        private readonly DomainMapper domainMapper;
        private readonly ConstraintMapper constraintMapper;
        private readonly ConnectionMapper connectionMapper;
        private readonly ConnectorMapper connectorMapper;
        private readonly ModelViewModelCache cache;

        internal ModelMapper(ModelViewModelCache theWorkspaceMapper)
        {
            this.cache = theWorkspaceMapper;
            this.connectorMapper = new ConnectorMapper(this.cache);
            this.variableMapper = new VariableMapper(this.cache);
            this.domainMapper = new DomainMapper(this.cache);
            this.constraintMapper = new ConstraintMapper(this.cache);
            this.connectionMapper = new ConnectionMapper(this.cache);
        }

        internal ModelViewModel MapFrom(ModelModel theModelModel)
        {
            var modelViewModel = new ModelViewModel();
            modelViewModel.Model = theModelModel;

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
                modelViewModel.FixupVariable(variableViewModel);
            }

            foreach (var connectionModel in theModelModel.Connections)
            {
                var connectionViewModel = this.connectionMapper.MapFrom(connectionModel);
                this.connectorMapper.FixupFrom(connectionModel);
                modelViewModel.FixupConnection(connectionViewModel);
            }

            return modelViewModel;
        }
    }
}