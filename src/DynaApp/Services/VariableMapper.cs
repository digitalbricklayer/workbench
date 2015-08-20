using System.Diagnostics;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class VariableMapper
    {
        private readonly ModelViewModelCache cache;
        private readonly ConnectorMapper connectorMapper;

        internal VariableMapper(ModelViewModelCache theCache)
        {
            this.connectorMapper =  new ConnectorMapper(theCache);
            this.cache = theCache;
        }

        internal VariableViewModel MapFrom(VariableModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new VariableViewModel
            {
                Id = theVariableModel.Id,
                Model = theVariableModel,
                Name = theVariableModel.Name,
                X = theVariableModel.X,
                Y = theVariableModel.Y
            };

            foreach (var connectorModel in theVariableModel.Connectors)
            {
                var connectorViewModel = this.connectorMapper.MapFrom(connectorModel);
                variableViewModel.AddConnector(connectorViewModel);
            }

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }
    }
}