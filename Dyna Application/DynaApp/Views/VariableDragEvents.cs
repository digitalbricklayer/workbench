using System.Collections;
using System.Windows;

namespace DynaApp.Views
{
    /// <summary>
    /// Base class for variable dragging event args.
    /// </summary>
    public class VariableDragEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The VariableItem's or their DataContext (when non-NULL).
        /// </summary>
        public ICollection variables = null;

        protected VariableDragEventArgs(RoutedEvent routedEvent, object source, ICollection variables) :
            base(routedEvent, source)
        {
            this.variables = variables;
        }

        /// <summary>
        /// The VariableItem's or their DataContext (when non-NULL).
        /// </summary>
        public ICollection Variables
        {
            get
            {
                return variables;
            }
        }
    }

    /// <summary>
    /// Defines the event handler for VariableDragStarted events.
    /// </summary>
    public delegate void VariableDragEventHandler(object sender, VariableDragEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user starts to drag a variable in the network.
    /// </summary>
    public class VariableDragStartedEventArgs : VariableDragEventArgs
    {
        /// <summary>
        /// Set to 'false' to disallow dragging.
        /// </summary>
        private bool cancel = false;

        internal VariableDragStartedEventArgs(RoutedEvent routedEvent, object source, ICollection variables) :
            base(routedEvent, source, variables)
        {
        }

        /// <summary>
        /// Set to 'false' to disallow dragging.
        /// </summary>
        public bool Cancel
        {
            get
            {
                return cancel;
            }
            set
            {
                cancel = value;
            }
        }
    }

    /// <summary>
    /// Defines the event handler for VariableDragStarted events.
    /// </summary>
    public delegate void VariableDragStartedEventHandler(object sender, VariableDragStartedEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a variable in the network.
    /// </summary>
    public class VariableDraggingEventArgs : VariableDragEventArgs
    {
        /// <summary>
        /// The amount the variable has been dragged horizontally.
        /// </summary>
        public double horizontalChange = 0;

        /// <summary>
        /// The amount the variable has been dragged vertically.
        /// </summary>
        public double verticalChange = 0;

        internal VariableDraggingEventArgs(RoutedEvent routedEvent, object source, ICollection variables, double horizontalChange, double verticalChange) :
            base(routedEvent, source, variables)
        {
            this.horizontalChange = horizontalChange;
            this.verticalChange = verticalChange;
        }

        /// <summary>
        /// The amount the variable has been dragged horizontally.
        /// </summary>
        public double HorizontalChange
        {
            get
            {
                return horizontalChange;
            }
        }

        /// <summary>
        /// The amount the variable has been dragged vertically.
        /// </summary>
        public double VerticalChange
        {
            get
            {
                return verticalChange;
            }
        }
    }

    /// <summary>
    /// Defines the event handler for VariableDragStarted events.
    /// </summary>
    public delegate void VariableDraggingEventHandler(object sender, VariableDraggingEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user has completed dragging a variable in the network.
    /// </summary>
    public class VariableDragCompletedEventArgs : VariableDragEventArgs
    {
        public VariableDragCompletedEventArgs(RoutedEvent routedEvent, object source, ICollection variables) :
            base(routedEvent, source, variables)
        {
        }
    }

    /// <summary>
    /// Defines the event handler for VariableDragCompleted events.
    /// </summary>
    public delegate void VariableDragCompletedEventHandler(object sender, VariableDragCompletedEventArgs e);
}
