using System.Collections;
using System.Windows;

namespace DynaApp.Controls
{
    /// <summary>
    /// Base class for variable dragging event args.
    /// </summary>
    public class DomainDragEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The VariableItem's or their DataContext (when non-NULL).
        /// </summary>
        public ICollection domains;

        protected DomainDragEventArgs(RoutedEvent routedEvent, object source, ICollection domains)
            : base(routedEvent, source)
        {
            this.domains = domains;
        }

        /// <summary>
        /// The VariableItem's or their DataContext (when non-NULL).
        /// </summary>
        public ICollection Domains
        {
            get
            {
                return domains;
            }
        }
    }

    /// <summary>
    /// Defines the event handler for VariableDragStarted events.
    /// </summary>
    public delegate void DomainDragEventHandler(object sender, DomainDragEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user starts to drag a variable in the network.
    /// </summary>
    public class DomainDragStartedEventArgs : DomainDragEventArgs
    {
        /// <summary>
        /// Set to 'false' to disallow dragging.
        /// </summary>
        private bool cancel;

        internal DomainDragStartedEventArgs(RoutedEvent routedEvent, object source, ICollection domains) :
            base(routedEvent, source, domains)
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
    public delegate void DomainDragStartedEventHandler(object sender, DomainDragStartedEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a variable in the network.
    /// </summary>
    public class DomainDraggingEventArgs : DomainDragEventArgs
    {
        /// <summary>
        /// The amount the variable has been dragged horizontally.
        /// </summary>
        public double horizontalChange;

        /// <summary>
        /// The amount the variable has been dragged vertically.
        /// </summary>
        public double verticalChange;

        internal DomainDraggingEventArgs(RoutedEvent routedEvent, object source, ICollection domains, double horizontalChange, double verticalChange) :
            base(routedEvent, source, domains)
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
    public delegate void DomainDraggingEventHandler(object sender, DomainDraggingEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user has completed dragging a variable in the network.
    /// </summary>
    public class DomainDragCompletedEventArgs : DomainDragEventArgs
    {
        public DomainDragCompletedEventArgs(RoutedEvent routedEvent, object source, ICollection domains) :
            base(routedEvent, source, domains)
        {
        }
    }

    /// <summary>
    /// Defines the event handler for VariableDragCompleted events.
    /// </summary>
    public delegate void DomainDragCompletedEventHandler(object sender, DomainDragCompletedEventArgs e);
}
