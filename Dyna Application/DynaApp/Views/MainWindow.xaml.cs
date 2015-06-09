using System.Windows;
using System.Windows.Input;
using DynaApp.ViewModels;

namespace DynaApp.Views
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
            var curDragPoint = Mouse.GetPosition(modelControl);

            //
            // Delegate the real work to the view model.
            //
            var connection = this.ViewModel.Model.ConnectionDragStarted(draggedOutConnector, curDragPoint);

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
            var curDragPoint = Mouse.GetPosition(modelControl);
            var connection = (ConnectionViewModel)e.Connection;
            this.ViewModel.Model.ConnectionDragging(connection, curDragPoint);
        }

        /// <summary>
        /// Event raised when the user has finished dragging out a connection.
        /// </summary>
        private void modelControl_ConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
        {
            var connectorDraggedOut = (ConnectorViewModel)e.ConnectorDraggedOut;
            var connectorDraggedOver = (ConnectorViewModel)e.ConnectorDraggedOver;
            var newConnection = (ConnectionViewModel)e.Connection;
            this.ViewModel.Model.ConnectionDragCompleted(newConnection, connectorDraggedOut, connectorDraggedOver);
        }

        /// <summary>
        /// Event raised to delete the selected graphics.
        /// </summary>
        private void DeleteSelectedGraphics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewModel.DeleteSelectedGraphics();
        }

        /// <summary>
        /// Event raised to close the application.
        /// </summary>
        private void FileCloseCommand(object sender, ExecutedRoutedEventArgs args)
        {
            this.Close();
        }

        /// <summary>
        /// Event raised to create a new variable.
        /// </summary>
        private void CreateVariable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var newVariableLocation = Mouse.GetPosition(modelControl);
            this.ViewModel.CreateVariable("New Variable!", newVariableLocation);
        }
    }
}
