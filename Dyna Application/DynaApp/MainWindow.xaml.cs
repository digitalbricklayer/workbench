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
#if false
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var curDragPoint = Mouse.GetPosition(contentView);

            //
            // Delegate the real work to the view model.
            //
            var connection = this.ViewModel.Model.ConnectionDragStarted(draggedOutConnector, curDragPoint);

            //
            // Must return the view-model object that represents the connection via the event args.
            // This is so that NetworkView can keep track of the object while it is being dragged.
            //
            e.Connection = connection;

#endif
        }

        /// <summary>
        /// Event raised while the user is dragging a connection.
        /// </summary>
        private void modelControl_ConnectionDragging(object sender, ConnectionDraggingEventArgs e)
        {
#if false
            var curDragPoint = Mouse.GetPosition(contentView);
            var connection = (ConnectionViewModel)e.Connection;
            this.ViewModel.Model.ConnectionDragging(connection, curDragPoint);

#endif
        }

        /// <summary>
        /// Event raised, to query for feedback, while the user is dragging a connection.
        /// </summary>
        private void modelControl_QueryConnectionFeedback(object sender, QueryConnectionFeedbackEventArgs e)
        {
#if false
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var draggedOverConnector = (ConnectorViewModel)e.DraggedOverConnector;
            object feedbackIndicator;
            bool connectionOk;

            this.ViewModel.Model.QueryConnnectionFeedback(draggedOutConnector, draggedOverConnector, out feedbackIndicator, out connectionOk);

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

#endif
        }

        /// <summary>
        /// Event raised when the user has finished dragging out a connection.
        /// </summary>
        private void modelControl_ConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
        {
#if false
            var connectorDraggedOut = (ConnectorViewModel)e.ConnectorDraggedOut;
            var connectorDraggedOver = (ConnectorViewModel)e.ConnectorDraggedOver;
            var newConnection = (ConnectionViewModel)e.Connection;
            this.ViewModel.Model.ConnectionDragCompleted(newConnection, connectorDraggedOut, connectorDraggedOver);

#endif
        }

        /// <summary>
        /// Event raised to delete the selected graphics.
        /// </summary>
        private void DeleteSelectedGraphics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
//            this.ViewModel.DeleteSelectedGraphics();
        }

        private void DeleteConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
#if false
            var connection = (ConnectionViewModel)e.Parameter;
            this.ViewModel.Model.DeleteConnection(connection);

#endif
        }

        private void DeleteVariable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
#if false
            var variable = (VariableViewModel)e.Parameter;
            this.ViewModel.Model.DeleteVariable(variable);

#endif
        }

        private void DeleteDomain_Executed(object sender, ExecutedRoutedEventArgs e)
        {
#if false
            var domain = (DomainViewModel)e.Parameter;
            this.ViewModel.Model.DeleteDomain(domain);

#endif
        }

        private void DeleteConstraint_Executed(object sender, ExecutedRoutedEventArgs e)
        {
#if false
            var constraint = (ConstraintViewModel)e.Parameter;
            this.ViewModel.Model.DeleteConstraint(constraint);

#endif
        }
    }
}
