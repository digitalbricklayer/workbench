using System.Collections;
using System.Windows;

namespace DynaApp.Events
{
    /// <summary>
    /// Base class for constraint dragging event args.
    /// </summary>
    public class ConstraintDragEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The VariableItem's or their DataContext (when non-NULL).
        /// </summary>
        public ICollection constraints;

        protected ConstraintDragEventArgs(RoutedEvent routedEvent, object source, ICollection constraints) :
            base(routedEvent, source)
        {
            this.constraints = constraints;
        }

        /// <summary>
        /// The VariableItem's or their DataContext (when non-NULL).
        /// </summary>
        public ICollection Constraints
        {
            get
            {
                return constraints;
            }
        }
    }

    /// <summary>
    /// Defines the event handler for ConstraintDragStarted events.
    /// </summary>
    public delegate void ConstraintDragEventHandler(object sender, ConstraintDragEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user starts to drag a variable in the network.
    /// </summary>
    public class ConstraintDragStartedEventArgs : ConstraintDragEventArgs
    {
        /// <summary>
        /// Set to 'false' to disallow dragging.
        /// </summary>
        private bool cancel;

        internal ConstraintDragStartedEventArgs(RoutedEvent routedEvent, object source, ICollection constraints) :
            base(routedEvent, source, constraints)
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
    public delegate void ConstraintDragStartedEventHandler(object sender, ConstraintDragStartedEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a variable in the network.
    /// </summary>
    public class ConstraintDraggingEventArgs : ConstraintDragEventArgs
    {
        /// <summary>
        /// The amount the variable has been dragged horizontally.
        /// </summary>
        public double horizontalChange;

        /// <summary>
        /// The amount the variable has been dragged vertically.
        /// </summary>
        public double verticalChange;

        internal ConstraintDraggingEventArgs(RoutedEvent routedEvent, object source, ICollection constraints, double horizontalChange, double verticalChange) :
            base(routedEvent, source, constraints)
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
    /// Defines the event handler for ConstraintDragStarted events.
    /// </summary>
    public delegate void ConstraintDraggingEventHandler(object sender, ConstraintDraggingEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user has completed dragging a variable in the network.
    /// </summary>
    public class ConstraintDragCompletedEventArgs : ConstraintDragEventArgs
    {
        public ConstraintDragCompletedEventArgs(RoutedEvent routedEvent, object source, ICollection constraints)
            : base(routedEvent, source, constraints)
        {
        }
    }

    /// <summary>
    /// Defines the event handler for VariableDragCompleted events.
    /// </summary>
    public delegate void ConstraintDragCompletedEventHandler(object sender, ConstraintDragCompletedEventArgs e);
}
