using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DynaApp.Views
{
    public class VariableItem : ListBoxItem
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(VariableItem),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(VariableItem),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ZIndexProperty =
            DependencyProperty.Register("ZIndex", typeof(int), typeof(VariableItem),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        internal static readonly DependencyProperty ParentModelViewProperty =
            DependencyProperty.Register("ParentModelView", typeof(ModelView), typeof(VariableItem), 
                new FrameworkPropertyMetadata(ParentModelView_PropertyChanged));

        internal static readonly RoutedEvent VariableDragStartedEvent =
            EventManager.RegisterRoutedEvent("VariableDragStarted", RoutingStrategy.Bubble, typeof(VariableDragStartedEventHandler), typeof(VariableItem));

        internal static readonly RoutedEvent VariableDraggingEvent =
            EventManager.RegisterRoutedEvent("VariableDragging", RoutingStrategy.Bubble, typeof(VariableDraggingEventHandler), typeof(VariableItem));

        internal static readonly RoutedEvent VariableDragCompletedEvent =
            EventManager.RegisterRoutedEvent("VariableDragCompleted", RoutingStrategy.Bubble, typeof(VariableDragCompletedEventHandler), typeof(VariableItem));

        #endregion Dependency Property/Event Definitions

        public VariableItem()
        {
            //
            // By default, we don't want this UI element to be focusable.
            //
            Focusable = false;
        }

        /// <summary>
        /// The X coordinate of the Variable.
        /// </summary>
        public double X
        {
            get
            {
                return (double)GetValue(XProperty);
            }
            set
            {
                SetValue(XProperty, value);
            }
        }

        /// <summary>
        /// The Y coordinate of the variable.
        /// </summary>
        public double Y
        {
            get
            {
                return (double)GetValue(YProperty);
            }
            set
            {
                SetValue(YProperty, value);
            }
        }

        /// <summary>
        /// The Z index of the variable.
        /// </summary>
        public int ZIndex
        {
            get
            {
                return (int)GetValue(ZIndexProperty);
            }
            set
            {
                SetValue(ZIndexProperty, value);
            }
        }

        /// <summary>
        /// Reference to the data-bound parent ModelView.
        /// </summary>
        internal ModelView ParentModelView
        {
            get
            {
                return (ModelView)GetValue(ParentModelViewProperty);
            }
            set
            {
                SetValue(ParentModelViewProperty, value);
            }
        }

        /// <summary>
        /// The point the mouse was last at when dragging.
        /// </summary>
        private Point lastMousePoint;

        /// <summary>
        /// Set to 'true' when left mouse button is held down.
        /// </summary>
        private bool isLeftMouseDown;

        /// <summary>
        /// Set to 'true' when left mouse button and the control key are held down.
        /// </summary>
        private bool isLeftMouseAndControlDown;

        /// <summary>
        /// Set to 'true' when dragging has started.
        /// </summary>
        private bool isDragging;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before dragging begins.
        /// </summary>
        private const double DragThreshold = 5;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static VariableItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VariableItem), new FrameworkPropertyMetadata(typeof(VariableItem)));
        }

        /// <summary>
        /// Bring the variable to the front of other elements.
        /// </summary>
        internal void BringToFront()
        {
            if (this.ParentModelView == null) return;

            int maxZ = this.ParentModelView.FindMaxZIndex();
            this.ZIndex = maxZ + 1;
        }

        /// <summary>
        /// Called when a mouse button is held down.
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            BringToFront();

            if (this.ParentModelView != null)
            {
                this.ParentModelView.Focus();
            }

            if (e.ChangedButton == MouseButton.Left && this.ParentModelView != null)
            {
                lastMousePoint = e.GetPosition(this.ParentModelView);
                isLeftMouseDown = true;

                LeftMouseDownSelectionLogic();

                e.Handled = true;
            }
            else if (e.ChangedButton == MouseButton.Right && this.ParentModelView != null)
            {
                RightMouseDownSelectionLogic();
           }
        }

        /// <summary>
        /// This method contains selection logic that is invoked when the left mouse button is pressed down.
        /// The reason this exists in its own method rather than being included in OnMouseDown is 
        /// so that ConnectorItem can reuse this logic from its OnMouseDown.
        /// </summary>
        internal void LeftMouseDownSelectionLogic()
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                //
                // Control key was held down.
                // This means that the rectangle is being added to or removed from the existing selection.
                // Don't do anything yet, we will act on this later in the MouseUp event handler.
                //
                isLeftMouseAndControlDown = true;
            }
            else
            {
                //
                // Control key is not held down.
                //
                isLeftMouseAndControlDown = false;

                if (this.ParentModelView.SelectedVariables.Count == 0)
                {
                    //
                    // Nothing already selected, select the item.
                    //
                    this.IsSelected = true;
                }
                else if (this.ParentModelView.SelectedVariables.Contains(this) ||
                         this.ParentModelView.SelectedVariables.Contains(this.DataContext))
                {
                    // 
                    // Item is already selected, do nothing.
                    // We will act on this in the MouseUp if there was no drag operation.
                    //
                }
                else
                {
                    //
                    // Item is not selected.
                    // Deselect all, and select the item.
                    //
                    this.ParentModelView.SelectedVariables.Clear();
                    this.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// This method contains selection logic that is invoked when the right mouse button is pressed down.
        /// The reason this exists in its own method rather than being included in OnMouseDown is 
        /// so that ConnectorItem can reuse this logic from its OnMouseDown.
        /// </summary>
        internal void RightMouseDownSelectionLogic()
        {
            if (this.ParentModelView.SelectedVariables.Count == 0)
            {
                //
                // Nothing already selected, select the item.
                //
                this.IsSelected = true;
            }
            else if (this.ParentModelView.SelectedVariables.Contains(this) ||
                     this.ParentModelView.SelectedVariables.Contains(this.DataContext))
            {
                // 
                // Item is already selected, do nothing.
                //
            }
            else
            {
                //
                // Item is not selected.
                // Deselect all, and select the item.
                //
                this.ParentModelView.SelectedVariables.Clear();
                this.IsSelected = true;
            }
        }

        /// <summary>
        /// Called when the mouse cursor is moved.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDragging)
            {
                //
                // Raise the event to notify that dragging is in progress.
                //

                Point curMousePoint = e.GetPosition(this.ParentModelView);

                object item = this;
                if (DataContext != null)
                {
                    item = DataContext;
                }

                Vector offset = curMousePoint - lastMousePoint;
                if (offset.X != 0.0 ||
                    offset.Y != 0.0)
                {
                    lastMousePoint = curMousePoint;

                    RaiseEvent(new VariableDraggingEventArgs(VariableDraggingEvent, this, new object[] { item }, offset.X, offset.Y));
                }
            }
            else if (isLeftMouseDown && this.ParentModelView.EnableVariableDragging)
            {
                //
                // The user is left-dragging the variable,
                // but don't initiate the drag operation until 
                // the mouse cursor has moved more than the threshold distance.
                //
                Point curMousePoint = e.GetPosition(this.ParentModelView);
                var dragDelta = curMousePoint - lastMousePoint;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance > DragThreshold)
                {
                    //
                    // When the mouse has been dragged more than the threshold value commence dragging the variable.
                    //

                    //
                    // Raise an event to notify that that dragging has commenced.
                    //
                    VariableDragStartedEventArgs eventArgs = new VariableDragStartedEventArgs(VariableDragStartedEvent, this, new VariableItem[] { this });
                    RaiseEvent(eventArgs);

                    if (eventArgs.Cancel)
                    {
                        //
                        // Handler of the event disallowed dragging of the variable.
                        //
                        isLeftMouseDown = false;
                        isLeftMouseAndControlDown = false;
						return;
                    }

                    isDragging = true;
                    this.CaptureMouse();
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (isLeftMouseDown)
            {
                if (isDragging)
                {
                    //
                    // Raise an event to notify that variable dragging has finished.
                    //

                    RaiseEvent(new VariableDragCompletedEventArgs(VariableDragCompletedEvent, this, new VariableItem[] { this }));

					this.ReleaseMouseCapture();

                    isDragging = false;
                }
                else
                {
                    //
                    // Execute mouse up selection logic only if there was no drag operation.
                    //

                    LeftMouseUpSelectionLogic();
                }

                isLeftMouseDown = false;
                isLeftMouseAndControlDown = false;

                e.Handled = true;
            }
        }

        /// <summary>
        /// This method contains selection logic that is invoked when the left mouse button is released.
        /// The reason this exists in its own method rather than being included in OnMouseUp is 
        /// so that ConnectorItem can reuse this logic from its OnMouseUp.
        /// </summary>
        internal void LeftMouseUpSelectionLogic()
        {
            if (isLeftMouseAndControlDown)
            {
                //
                // Control key was held down.
                // Toggle the selection.
                //
                this.IsSelected = !this.IsSelected;
            }
            else
            {
                //
                // Control key was not held down.
                //
                if (this.ParentModelView.SelectedVariables.Count == 1 &&
                    (this.ParentModelView.SelectedVariable == this ||
                     this.ParentModelView.SelectedVariable == this.DataContext))
                {
                    //
                    // The item that was clicked is already the only selected item.
                    // Don't need to do anything.
                    //
                }
                else
                {
                    //
                    // Clear the selection and select the clicked item as the only selected item.
                    //
                    this.ParentModelView.SelectedVariables.Clear();
                    this.IsSelected = true;
                }
            }

            isLeftMouseAndControlDown = false;
        }

        /// <summary>
        /// Event raised when the ParentModelView property has changed.
        /// </summary>
        private static void ParentModelView_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            //
            // Bring new variables to the front of the z-order.
            //
            var variableItem = (VariableItem) o;
            variableItem.BringToFront();
        }
    }
}
