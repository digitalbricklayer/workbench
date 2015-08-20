using System;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ConnectorMapper
    {
        private readonly ModelViewModelCache cache;

        internal ConnectorMapper(ModelViewModelCache theCache)
        {
            if (theCache == null)
                throw new ArgumentNullException("theCache");
            this.cache = theCache;
        }
        
        internal ConnectorViewModel MapFrom(ConnectorModel connectorModel)
        {
            var connectorViewModel = new ConnectorViewModel();
            connectorViewModel.ConnectorIdentity = connectorModel.Id;
            connectorViewModel.Hotspot = connectorModel.Hotspot;
            connectorViewModel.Model = connectorModel;

            this.cache.CacheConnector(connectorViewModel);

            return connectorViewModel;
        }

        /// <summary>
        /// Fix up the connector connection information from a connection model.
        /// </summary>
        /// <param name="connectionModel">A connection model.</param>
        internal void FixupFrom(ConnectionModel connectionModel)
        {
            var sourceConnectorViewModel = this.cache.GetConnectorByIdentity(connectionModel.SourceConnector.Id);
            sourceConnectorViewModel.AttachedConnection = this.cache.GetConnectionByIdentity(connectionModel.Id);
            sourceConnectorViewModel.Parent = this.cache.GetGraphicByIdentity(connectionModel.SourceConnector.Parent.Id);

            var destinationConnectorViewModel = this.cache.GetConnectorByIdentity(connectionModel.DestinationConnector.Id);
            destinationConnectorViewModel.AttachedConnection = this.cache.GetConnectionByIdentity(connectionModel.Id);
            destinationConnectorViewModel.Parent = this.cache.GetGraphicByIdentity(connectionModel.DestinationConnector.Parent.Id);
        }
    }
}
