using System;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ConnectionMapper
    {
        private readonly ModelViewModelCache cache;

        internal ConnectionMapper(ModelViewModelCache theCache)
        {
            if (theCache == null)
                throw new ArgumentNullException("theCache");
            this.cache = theCache;
        }

        internal ConnectionViewModel MapFrom(ConnectionModel connectionModel)
        {
            if (connectionModel == null) return null;
            var connectionViewModel = new ConnectionViewModel();
            connectionViewModel.Model = connectionModel;
            connectionViewModel.SourceConnector =
                this.cache.GetConnectorByIdentity(connectionModel.SourceConnector.Id);
            connectionViewModel.SourceConnectorHotspot = connectionModel.SourceConnectorHotspot;
            connectionViewModel.DestinationConnector =
                this.cache.GetConnectorByIdentity(connectionModel.DestinationConnector.Id);
            connectionViewModel.DestinationConnectorHotspot = connectionModel.DestinationConnectorHotspot;

            this.cache.CacheConnection(connectionViewModel);

            return connectionViewModel;
        }
    }
}