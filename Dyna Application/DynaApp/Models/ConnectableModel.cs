using System;
using System.Collections.Generic;

namespace DynaApp.Models
{
    [Serializable]
    public abstract class ConnectableModel
    {
        /// <summary>
        /// Initialize a connectable model with a name.
        /// </summary>
        /// <param name="connectableName">Connectable name.</param>
        protected ConnectableModel(string connectableName)
        {
            this.Name = connectableName;
        }

        /// <summary>
        /// Intialize a connectable model with default values.
        /// </summary>
        protected ConnectableModel()
        {
            this.Connectors = new List<ConnectorModel>();
        }

        public string Name { get; set; }

        public List<ConnectorModel> Connectors { get; private set; }
    }
}
