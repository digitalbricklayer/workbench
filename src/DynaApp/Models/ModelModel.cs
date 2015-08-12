using System;
using System.Collections.Generic;
using System.Linq;

namespace DynaApp.Models
{
    /// <summary>
    /// The model model.
    /// </summary>
    [Serializable]
    public class ModelModel
    {
        public List<VariableModel> Variables { get; set; }
        public List<DomainModel> Domains { get; set; }
        public List<ConstraintModel> Constraints { get; set; }
        public List<ConnectionModel> Connections { get; set; }

        public ModelModel()
        {
            this.Variables = new List<VariableModel>();
            this.Domains = new List<DomainModel>();
            this.Constraints = new List<ConstraintModel>();
            this.Connections = new List<ConnectionModel>();
        }

        public void AddConstraint(ConstraintModel constraint)
        {
            this.Constraints.Add(constraint);
        }

        public void AddVariable(VariableModel variableModel)
        {
            this.Variables.Add(variableModel);
        }

        public void AddDomain(DomainModel domain)
        {
            this.Domains.Add(domain);
        }

        /// <summary>
        /// Connect the variable to the graphic.
        /// </summary>
        /// <param name="variableModel">Variable.</param>
        /// <param name="endPoint">Graphic.</param>
        public void Connect(VariableModel variableModel, GraphicModel endPoint)
        {
            var newConnection = new ConnectionModel();
            var sourceConnector = this.AssignConnector(endPoint);
            var destinationConnector = this.AssignConnector(variableModel);
            newConnection.SourceConnector = sourceConnector;
            newConnection.DestinationConnector = destinationConnector;
            newConnection.DestinationConnector.AttachedConnection = newConnection;
            newConnection.SourceConnector.AttachedConnection = newConnection;
            newConnection.Connect(sourceConnector, destinationConnector);
            this.Connections.Add(newConnection);
        }

        /// <summary>
        /// Get the variable matching the variable name.
        /// </summary>
        /// <param name="theVariableName">The variable name.</param>
        /// <returns>Variable model.</returns>
        public VariableModel GetVariableByName(string theVariableName)
        {
            return this.Variables.FirstOrDefault(variable => variable.Name == theVariableName);
        }

        /// <summary>
        /// Assign an available connector.
        /// </summary>
        /// <param name="theGraphic">Graphic containing the connectors.</param>
        /// <returns>Available connector.</returns>
        private ConnectorModel AssignConnector(GraphicModel theGraphic)
        {
            return theGraphic.Connectors.FirstOrDefault(x => x.AttachedConnection == null);
        }
    }
}