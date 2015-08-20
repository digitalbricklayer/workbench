using System;
using System.Windows;

namespace DynaApp.Models
{
    [Serializable]
    public class ConnectionModel : ModelBase
    {
        public ConnectorModel SourceConnector { get; set; }
        public ConnectorModel DestinationConnector { get; set; }
        public Point SourceConnectorHotspot { get; set; }
        public Point DestinationConnectorHotspot { get; set; }

        /// <summary>
        /// Connect the two connectors.
        /// </summary>
        /// <param name="sourceConnector">Source connector.</param>
        /// <param name="destinationConnector">Destination connector.</param>
        public void Connect(ConnectorModel sourceConnector, ConnectorModel destinationConnector)
        {
            this.SourceConnector = sourceConnector;
            this.DestinationConnector = destinationConnector;
        }
    }
}
