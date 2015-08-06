using System.Windows;

namespace DynaApp.Events
{
    /// <summary>
    /// Base class for connection dragging event args.
    /// </summary>
    public class ConnectionDragEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The VariableItem or it's DataContext (when non-NULL).
        /// </summary>
        private object variable = null;

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        private object draggedOutConnector = null;

        /// <summary>
        /// The connector that will be dragged out.
        /// </summary>
        protected object connection = null;

        /// <summary>
        /// The VariableItem or it's DataContext (when non-NULL).
        /// </summary>
        public object Variable
        {
            get
            {
                return variable;
            }
        }

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        public object ConnectorDraggedOut
        {
            get
            {
                return draggedOutConnector;
            }
        }

        protected ConnectionDragEventArgs(RoutedEvent routedEvent, object source, object variable, object connection, object connector) :
            base(routedEvent, source)
        {
            this.variable = variable;
            this.draggedOutConnector = connector;
            this.connection = connection;
        }
    }

    /// <summary>
    /// Arguments for event raised when the user starts to drag a connection out from a variable.
    /// </summary>
    public class ConnectionDragStartedEventArgs : ConnectionDragEventArgs
    {
        /// <summary>
        /// The connection that will be dragged out.
        /// </summary>
        public object Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value;
            }
        }

        internal ConnectionDragStartedEventArgs(RoutedEvent routedEvent, object source, object variable, object connector) :
            base(routedEvent, source, variable, null, connector)
        {
        }
    }

    /// <summary>
    /// Defines the event handler for the ConnectionDragStarted event.
    /// </summary>
    public delegate void ConnectionDragStartedEventHandler(object sender, ConnectionDragStartedEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a variable in the network.
    /// </summary>
    public class QueryConnectionFeedbackEventArgs : ConnectionDragEventArgs
    {
        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        private object draggedOverConnector = null;

        /// <summary>
        /// Set to 'true' / 'false' to indicate that the connection from the dragged out connection to the dragged over connector is valid.
        /// </summary>
        private bool connectionOk = true;

        /// <summary>
        /// The indicator to display.
        /// </summary>
        private object feedbackIndicator = null;

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        public object DraggedOverConnector
        {
            get
            {
                return draggedOverConnector;
            }
        }

        /// <summary>
        /// The connection that will be dragged out.
        /// </summary>
        public object Connection
        {
            get
            {
                return connection;
            }
        }

        /// <summary>
        /// Set to 'true' / 'false' to indicate that the connection from the dragged out connection to the dragged over connector is valid.
        /// </summary>
        public bool ConnectionOk
        {
            get
            {
                return connectionOk;
            }
            set
            {
                connectionOk = value;
            }
        }

        /// <summary>
        /// The indicator to display.
        /// </summary>
        public object FeedbackIndicator
        {
            get
            {
                return feedbackIndicator;
            }
            set
            {
                feedbackIndicator = value;
            }
        }

        internal QueryConnectionFeedbackEventArgs(RoutedEvent routedEvent, object source,
            object variable, object connection, object connector, object draggedOverConnector) :
            base(routedEvent, source, variable, connection, connector)
        {
            this.draggedOverConnector = draggedOverConnector;
        }
    }

    /// <summary>
    /// Defines the event handler for the QueryConnectionFeedback event.
    /// </summary>
    public delegate void QueryConnectionFeedbackEventHandler(object sender, QueryConnectionFeedbackEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a variable in the network.
    /// </summary>
    public class ConnectionDraggingEventArgs : ConnectionDragEventArgs
    {
        /// <summary>
        /// The connection being dragged out.
        /// </summary>
        public object Connection
        {
            get
            {
                return connection;
            }
        }

        internal ConnectionDraggingEventArgs(RoutedEvent routedEvent, object source,
                object variable, object connection, object connector) :
            base(routedEvent, source, variable, connection, connector)
        {
        }
    }

    /// <summary>
    /// Defines the event handler for the ConnectionDragging event.
    /// </summary>
    public delegate void ConnectionDraggingEventHandler(object sender, ConnectionDraggingEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user has completed dragging a connector.
    /// </summary>
    public class ConnectionDragCompletedEventArgs : ConnectionDragEventArgs
    {
        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        private object connectorDraggedOver = null;

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        public object ConnectorDraggedOver
        {
            get
            {
                return connectorDraggedOver;
            }
        }

        /// <summary>
        /// The connection that will be dragged out.
        /// </summary>
        public object Connection
        {
            get
            {
                return connection;
            }
        }

        internal ConnectionDragCompletedEventArgs(RoutedEvent routedEvent, object source, object variable, object connection, object connector, object connectorDraggedOver) :
            base(routedEvent, source, variable, connection, connector)
        {
            this.connectorDraggedOver = connectorDraggedOver;
        }
    }

    /// <summary>
    /// Defines the event handler for the ConnectionDragCompleted event.
    /// </summary>
    public delegate void ConnectionDragCompletedEventHandler(object sender, ConnectionDragCompletedEventArgs e);
}
