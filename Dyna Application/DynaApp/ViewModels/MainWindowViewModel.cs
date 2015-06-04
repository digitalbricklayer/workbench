using System.Linq;
using System.Windows;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public sealed class MainWindowViewModel : AbstractModelBase
    {
        public MainWindowViewModel()
        {
            this.Model = new ModelViewModel();
            this.CreateVariable("Jack", new Point(10, 10));
            this.CreateVariable("Bob", new Point(200, 10));
        }

        /// <summary>
        /// Gets the model displayed in the main window.
        /// </summary>
        public ModelViewModel Model { get; private set; }

        public VariableViewModel CreateVariable(string newVariableName, Point newVariableLocation)
        {
            var newVariable = new VariableViewModel(newVariableName);
            newVariable.X = newVariableLocation.X;
            newVariable.Y = newVariableLocation.Y;

            this.Model.AddVariable(newVariable);

            return newVariable;
        }

        /// <summary>
        /// Called when the user has started to drag out a connector, thus creating a new connection.
        /// </summary>
        public ConnectionViewModel ConnectionDragStarted(ConnectorViewModel draggedOutConnector, Point curDragPoint)
        {
            if (draggedOutConnector.AttachedConnection != null)
            {
                //
                // There is an existing connection attached to the connector that has been dragged out.
                // Remove the existing connection from the view-model.
                //
                this.Model.Connections.Remove(draggedOutConnector.AttachedConnection);
            }

            //
            // Create a new connection to add to the view-model.
            //
            var connection = new ConnectionViewModel();

            //
            // Link the source connector to the connector that was dragged out.
            //
            connection.SourceConnector = draggedOutConnector;

            //
            // Set the position of destination connector to the current position of the mouse cursor.
            //
            connection.DestConnectorHotspot = curDragPoint;

            //
            // Add the new connection to the view-model.
            //
            this.Model.Connections.Add(connection);

            return connection;
        }

        /// <summary>
        /// Called as the user continues to drag the connection.
        /// </summary>
        public void ConnectionDragging(ConnectionViewModel connection, Point curDragPoint)
        {
            //
            // Update the destination connection hotspot while the user is dragging the connection.
            //
            connection.DestConnectorHotspot = curDragPoint;
        }

        /// <summary>
        /// Called when the user has finished dragging out the new connection.
        /// </summary>
        public void ConnectionDragCompleted(ConnectionViewModel newConnection, ConnectorViewModel connectorDraggedOut, ConnectorViewModel connectorDraggedOver)
        {
            if (connectorDraggedOver == null)
            {
                //
                // The connection was unsuccessful.
                // Maybe the user dragged it out and dropped it in empty space.
                //
                this.Model.Connections.Remove(newConnection);
                return;
            }

            //
            // The user has dragged the connection on top of another valid connector.
            //

            var existingConnection = connectorDraggedOver.AttachedConnection;
            if (existingConnection != null)
            {
                //
                // There is already a connection attached to the connector that was dragged over.
                // Remove the existing connection from the view-model.
                //
                this.Model.Connections.Remove(existingConnection);
            }

            //
            // Finalize the connection by attaching it to the connector
            // that the user dropped the connection on.
            //
            newConnection.DestConnector = connectorDraggedOver;
        }

        /// <summary>
        /// Delete the currently selected variables from the view-model.
        /// </summary>
        public void DeleteSelectedVariables()
        {
            // Take a copy of the variables list so we can delete variables while iterating.
            var variablesCopy = this.Model.Variables.ToArray();

            foreach (var variable in variablesCopy)
            {
                if (variable.IsSelected)
                {
                    DeleteVariable(variable);
                }
            }
        }

        /// <summary>
        /// Delete the variable from the view-model.
        /// Also deletes any connections to or from the variable.
        /// </summary>
        public void DeleteVariable(VariableViewModel variable)
        {
            //
            // Remove all connections attached to the variable.
            //
//            this.Model.Connections.RemoveRange(variable.AttachedConnections);

            //
            // Remove the variable from the network.
            //
            this.Model.Variables.Remove(variable);
        }
    }
}
