using System;
using System.Collections.Generic;

namespace DynaApp.Models
{
    [Serializable]
    public abstract class GraphicModel
    {
        /// <summary>
        /// Initialize a connectable model with a name.
        /// </summary>
        /// <param name="connectableName">Connectable name.</param>
        protected GraphicModel(string connectableName)
            : this()
        {
            this.Name = connectableName;
        }

        /// <summary>
        /// Intialize a connectable model with default values.
        /// </summary>
        protected GraphicModel()
        {
            this.Connectors = new List<ConnectorModel>();
        }

        public string Name { get; set; }

        public List<ConnectorModel> Connectors { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
