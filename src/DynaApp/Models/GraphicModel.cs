using System;
using System.Collections.Generic;

namespace DynaApp.Models
{
    [Serializable]
    public abstract class GraphicModel
    {
        /// <summary>
        /// Initialize a graphic model with a name.
        /// </summary>
        /// <param name="connectableName">Connectable name.</param>
        protected GraphicModel(string connectableName)
            : this()
        {
            this.Name = connectableName;
        }

        /// <summary>
        /// Initialize a graphic model with default values.
        /// </summary>
        protected GraphicModel()
        {
            this.Connectors = new List<ConnectorModel>();
            this.CreateConnectors();
        }

        public string Name { get; set; }

        public List<ConnectorModel> Connectors { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>
        /// Create the graphic connectors.
        /// </summary>
        private void CreateConnectors()
        {
            for (var i = 0; i < 4; i++)
                this.AddConnector(new ConnectorModel());
        }

        /// <summary>
        /// Add a connector
        /// </summary>
        /// <param name="newConnector">New connector.</param>
        private void AddConnector(ConnectorModel newConnector)
        {
            newConnector.Parent = this;
            this.Connectors.Add(newConnector);
        }
    }
}
