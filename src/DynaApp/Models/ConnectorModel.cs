using System;
using System.Windows;

namespace DynaApp.Models
{
    [Serializable]
    public class ConnectorModel
    {
        public ConnectionModel AttachedConnection { get; set; }
        public Point Hotspot { get; set; }
    }
}
