using System;
using System.Windows;

namespace DynaApp.Models
{
    /// <summary>
    /// The connector hosts a single connection.
    /// </summary>
    [Serializable]
    public class ConnectorModel
    {
        /// <summary>
        /// Initializes the connector with default values.
        /// </summary>
        public ConnectorModel()
        {
        }

        public ConnectionModel AttachedConnection { get; set; }
        public Point Hotspot { get; set; }

        /// <summary>
        /// Gets or sets the graphic the connector is attached to, or null if the 
        /// connector is not attached to anything.
        /// </summary>
        public GraphicModel Parent { get; set; }
    }
}
