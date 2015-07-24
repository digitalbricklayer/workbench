using System;
using System.Collections.Generic;

namespace DynaApp.Models
{
    [Serializable]
    public abstract class ConnectableModel
    {
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
