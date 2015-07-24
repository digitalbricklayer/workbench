using System.Windows;
using System.Windows.Input;
using DynaApp.Events;
using DynaApp.ViewModels;

namespace DynaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the main window view model.
        /// </summary>
        public MainWindowViewModel ViewModel
        {
            get
            {
                return (MainWindowViewModel)this.DataContext;
            }
        }

        /// <summary>
        /// Event raised when the user has started to drag out a connection.
        /// </summary>
        private void modelControl_ConnectionDragStarted(object sender, ConnectionDragStartedEventArgs e)
        {
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var curDragPoint = Mouse.GetPosition(this.workspaceView);

            //
            // Delegate the real work to the view model.
            //
            var connection = this.ViewModel.Workspace.Model.ConnectionDragStarted(draggedOutConnector, curDragPoint);

            //
            // Must return the view-model object that represents the connection via the event args.
            // This is so that NetworkView can keep track of the object while it is being dragged.
            //
            e.Connection = connection;
        }

        /// <summary>
        /// Event raised while the user is dragging a connection.
        /// </summary>
        private void modelControl_ConnectionDragging(object sender, ConnectionDraggingEventArgs e)
        {
            var curDragPoint = Mouse.GetPosition(this.workspaceView);
            var connection = (ConnectionViewModel)e.Connection;
            this.ViewModel.Workspace.Model.ConnectionDragging(connection, curDragPoint);
        }

        /// <summary>
        /// Event raised, to query for feedback, while the user is dragging a connection.
        /// </summary>
        private void modelControl_QueryConnectionFeedback(object sender, QueryConnectionFeedbackEventArgs e)
        {
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var draggedOverConnector = (ConnectorViewModel)e.DraggedOverConnector;
            object feedbackIndicator;
            bool connectionOk;

            this.ViewModel.Workspace.Model.QueryConnnectionFeedback(draggedOutConnector, draggedOverConnector, out feedbackIndicator, out connectionOk);

            //
            // Return the feedback object to NetworkView.
            // The object combined with the data-template for it will be used to create a 'feedback icon' to
            // display (in an adorner) to the user.
            //
            e.FeedbackIndicator = feedbackIndicator;

            //
            // Let NetworkView know if the connection is ok or not ok.
            //
            e.ConnectionOk = connectionOk;
        }

        /// <summary>
        /// Event raised when the user has finished dragging out a connection.
        /// </summary>
        private void modelControl_ConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
        {
            var connectorDraggedOut = (ConnectorViewModel)e.ConnectorDraggedOut;
            var connectorDraggedOver = (ConnectorViewModel)e.ConnectorDraggedOver;
            var newConnection = (ConnectionViewModel)e.Connection;
            this.ViewModel.Workspace.Model.ConnectionDragCompleted(newConnection, connectorDraggedOut, connectorDraggedOver);
        }

        /// <summary>
        /// Event raised to delete the selected graphics.
        /// </summary>
        private void DeleteSelectedGraphics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.Workspace.DeleteSelectedGraphics();
        }

        private void DeleteConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var connection = (ConnectionViewModel)e.Parameter;
            this.ViewModel.Workspace.Model.DeleteConnection(connection);
        }

        private void DeleteVariable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var variable = (VariableViewModel)e.Parameter;
            this.ViewModel.Workspace.Model.DeleteVariable(variable);
        }

        private void DeleteDomain_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var domain = (DomainViewModel)e.Parameter;
            this.ViewModel.Workspace.Model.DeleteDomain(domain);
        }

        private void DeleteConstraint_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var constraint = (ConstraintViewModel)e.Parameter;
            this.ViewModel.Workspace.Model.DeleteConstraint(constraint);
        }
    }
}
