using System.Collections.Generic;
using System.Diagnostics;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ModelViewModelCache
    {
        private readonly Dictionary<int, ConnectorViewModel> connectorMap;
        private readonly Dictionary<int, ConnectionViewModel> connectionMap;
        private readonly Dictionary<int, GraphicViewModel> graphicMap;
        private readonly Dictionary<int, VariableViewModel> variableMap;

        internal ModelViewModelCache()
        {
            this.connectorMap = new Dictionary<int, ConnectorViewModel>();
            this.connectionMap = new Dictionary<int, ConnectionViewModel>();
            this.graphicMap = new Dictionary<int, GraphicViewModel>();
            this.variableMap = new Dictionary<int, VariableViewModel>();
        }

        internal void CacheVariable(VariableViewModel variableViewModel)
        {
            Debug.Assert(variableViewModel.Id != default(int));
            this.CacheGraphic(variableViewModel);
            this.variableMap.Add(variableViewModel.Id, variableViewModel);
        }

        internal void CacheGraphic(GraphicViewModel graphicViewModel)
        {
            Debug.Assert(graphicViewModel.Id != default(int));
            this.graphicMap.Add(graphicViewModel.Id, graphicViewModel);
        }

        internal void CacheConnector(ConnectorViewModel connectorViewModel)
        {
            Debug.Assert(connectorViewModel.ConnectorIdentity != default(int));
            this.connectorMap.Add(connectorViewModel.ConnectorIdentity, connectorViewModel);
        }

        internal void CacheConnection(ConnectionViewModel connectionViewModel)
        {
            Debug.Assert(connectionViewModel.ConnectionIdentity != default(int));
            this.connectionMap.Add(connectionViewModel.ConnectionIdentity, connectionViewModel);
        }

        internal ConnectorViewModel GetConnectorByIdentity(int connectorIdentity)
        {
            Debug.Assert(connectorIdentity != default(int));
            return this.connectorMap[connectorIdentity];
        }

        internal ConnectionViewModel GetConnectionByIdentity(int connectionIdentity)
        {
            Debug.Assert(connectionIdentity != default(int));
            return this.connectionMap[connectionIdentity];
        }

        internal GraphicViewModel GetGraphicByIdentity(int graphicIdentity)
        {
            Debug.Assert(graphicIdentity != default(int));
            return this.graphicMap[graphicIdentity];
        }

        internal VariableViewModel GetVariableByIdentity(int variableIdentity)
        {
            Debug.Assert(variableIdentity != default(int));
            return this.variableMap[variableIdentity];
        }
    }
}
