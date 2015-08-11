using System;
using System.Windows;

namespace DynaApp.Models
{
    [Serializable]
    public class ConnectorModel
    {
        public ConnectionModel AttachedConnection { get; set; }
        public Point Hotspot { get; set; }

        /// <summary>
        /// Gets or sets the graphic the connector is attached to, or null if the 
        /// connector is not attached to anything.
        /// </summary>
        public GraphicModel Parent { get; set; }
    }
}
