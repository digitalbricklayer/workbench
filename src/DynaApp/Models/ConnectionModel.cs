using System;
using System.Windows;
using System.Windows.Media;

namespace DynaApp.Models
{
    [Serializable]
    public class ConnectionModel
    {
        public ConnectorModel SourceConnector { get; set; }
        public ConnectorModel DestinationConnector { get; set; }
        public Point SourceConnectorHotspot { get; set; }
        public Point DestinationConnectorHotspot { get; set; }
        public PointCollection Points { get; set; }
    }
}
