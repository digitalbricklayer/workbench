using System.Collections;
using System.Windows;

namespace DynaApp.Controls
{
    /// <summary>
    /// Base class for graphic dragging event args.
    /// </summary>
    public class GraphicDragEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The GraphicItems or their DataContext (when non-NULL).
        /// </summary>
        private readonly ICollection graphics;

        protected GraphicDragEventArgs(RoutedEvent routedEvent, object source, ICollection graphics) :
            base(routedEvent, source)
        {
            this.graphics = graphics;
        }

        /// <summary>
        /// Gets the graphic items.
        /// </summary>
        public ICollection Graphics
        {
            get
            {
                return graphics;
            }
        }
    }

    /// <summary>
    /// Defines the event handler for GraphicDragStarted events.
    /// </summary>
    public delegate void GraphicDragEventHandler(object sender, GraphicDragEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user starts to drag a graphic in the model.
    /// </summary>
    public class GraphicDragStartedEventArgs : GraphicDragEventArgs
    {
        /// <summary>
        /// Set to 'false' to disallow dragging.
        /// </summary>
        private bool cancel;

        internal GraphicDragStartedEventArgs(RoutedEvent routedEvent, object source, ICollection graphics) :
            base(routedEvent, source, graphics)
        {
        }

        /// <summary>
        /// Gets or sets the disallow dragging flag.
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
    public delegate void GraphicDragStartedEventHandler(object sender, GraphicDragStartedEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a graphic in the model.
    /// </summary>
    public class GraphicDraggingEventArgs : GraphicDragEventArgs
    {
        /// <summary>
        /// The amount the variable has been dragged horizontally.
        /// </summary>
        public double horizontalChange;

        /// <summary>
        /// The amount the variable has been dragged vertically.
        /// </summary>
        public double verticalChange;

        internal GraphicDraggingEventArgs(RoutedEvent routedEvent, object source, ICollection constraints, double horizontalChange, double verticalChange) :
            base(routedEvent, source, constraints)
        {
            this.horizontalChange = horizontalChange;
            this.verticalChange = verticalChange;
        }

        /// <summary>
        /// Gets the amount the graphic has been dragged horizontally.
        /// </summary>
        public double HorizontalChange
        {
            get
            {
                return horizontalChange;
            }
        }

        /// <summary>
        /// Gets the amount the graphic has been dragged vertically.
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
    /// Defines the event handler for GraphicDragStarted events.
    /// </summary>
    public delegate void GraphicDraggingEventHandler(object sender, GraphicDraggingEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user has completed dragging a graphic in the model.
    /// </summary>
    public class GraphicDragCompletedEventArgs : GraphicDragEventArgs
    {
        public GraphicDragCompletedEventArgs(RoutedEvent routedEvent, object source, ICollection graphic) :
            base(routedEvent, source, graphic)
        {
        }
    }

    /// <summary>
    /// Defines the event handler for GraphicDragCompleted events.
    /// </summary>
    public delegate void GraphicDragCompletedEventHandler(object sender, GraphicDragCompletedEventArgs e);
}
