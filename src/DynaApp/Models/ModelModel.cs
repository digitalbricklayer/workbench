using System;
using System.Collections.Generic;
using System.Linq;

namespace DynaApp.Models
{
    /// <summary>
    /// The model model.
    /// </summary>
    [Serializable]
    public class ModelModel : ModelBase
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

        public void AddConstraint(ConstraintModel newConstraint)
        {
            if (newConstraint == null)
                throw new ArgumentNullException("newConstraint");
            newConstraint.AssignIdentity();
            this.Constraints.Add(newConstraint);
        }

        /// <summary>
        /// Delete the constraint from the model.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ConstraintModel constraintToDelete)
        {
            if (constraintToDelete == null)
                throw new ArgumentNullException("constraintToDelete");
            this.Constraints.Remove(constraintToDelete);
        }

        public void AddVariable(VariableModel newVariable)
        {
            if (newVariable == null)
                throw new ArgumentNullException("newVariable");
            newVariable.AssignIdentity();
            this.Variables.Add(newVariable);
        }

        /// <summary>
        /// Delete the variable from the model.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableModel variableToDelete)
        {
            if (variableToDelete == null)
                throw new ArgumentNullException("variableToDelete");
            this.Variables.Remove(variableToDelete);
        }

        public void AddDomain(DomainModel newDomain)
        {
            if (newDomain == null)
                throw new ArgumentNullException("newDomain");
            newDomain.AssignIdentity();
            this.Domains.Add(newDomain);
        }

        /// <summary>
        /// Delete the domain from the model.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(DomainModel domainToDelete)
        {
            if (domainToDelete == null)
                throw new ArgumentNullException("domainToDelete");
            this.Domains.Remove(domainToDelete);
        }

        /// <summary>
        /// Add a connection.
        /// </summary>
        /// <param name="newConnectionModel">New connection.</param>
        public void AddConnection(ConnectionModel newConnectionModel)
        {
            if (newConnectionModel == null)
                throw new ArgumentNullException("newConnectionModel");
            this.Connections.Add(newConnectionModel);
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
            newConnection.AssignIdentity();
            this.AddConnection(newConnection);
        }

        /// <summary>
        /// Disconnect the connection.
        /// </summary>
        /// <param name="connectionModel">Connection to disconnect.</param>
        public void Disconnect(ConnectionModel connectionModel)
        {
            if (connectionModel == null)
                throw new ArgumentNullException("connectionModel");
            this.Connections.Remove(connectionModel);
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