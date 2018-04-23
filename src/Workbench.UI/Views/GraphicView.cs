using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Workbench.Controls;

namespace Workbench.Views
{
    /// <summary>
    /// Model view graphically displays the model.
    /// </summary>
    public class GraphicView : Control
    {
        #region Dependency Property/Event Definitions

        private static readonly DependencyPropertyKey GraphicsPropertyKey =
            DependencyProperty.RegisterReadOnly("Graphics", typeof(ObservableCollection<object>), typeof(GraphicView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty GraphicsProperty = GraphicsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty GraphicsSourceProperty =
            DependencyProperty.Register("GraphicsSource", typeof(IEnumerable), typeof(GraphicView),
                new FrameworkPropertyMetadata(GraphicsSource_PropertyChanged));

        public static readonly DependencyProperty IsClearSelectionOnEmptySpaceClickEnabledProperty =
            DependencyProperty.Register("IsClearSelectionOnEmptySpaceClickEnabled", typeof(bool), typeof(GraphicView),
                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty EnableGraphicDraggingProperty =
            DependencyProperty.Register("EnableGraphicDragging", typeof(bool), typeof(GraphicView),
                new FrameworkPropertyMetadata(true));

        private static readonly DependencyPropertyKey IsDraggingGraphicPropertyKey =
            DependencyProperty.RegisterReadOnly("IsDraggingGraphic", typeof(bool), typeof(GraphicView),
                new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsDraggingGraphicProperty = IsDraggingGraphicPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNotDraggingGraphicPropertyKey =
            DependencyProperty.RegisterReadOnly("IsNotDraggingGraphic", typeof(bool), typeof(GraphicView),
                new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsNotDraggingGraphicProperty = IsDraggingGraphicPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsDraggingPropertyKey =
            DependencyProperty.RegisterReadOnly("IsDragging", typeof(bool), typeof(GraphicView),
                new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNotDraggingPropertyKey =
            DependencyProperty.RegisterReadOnly("IsNotDragging", typeof(bool), typeof(GraphicView),
                new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsNotDraggingProperty = IsNotDraggingPropertyKey.DependencyProperty;

        public static readonly DependencyProperty GraphicItemTemplateProperty =
            DependencyProperty.Register("GraphicItemTemplate", typeof(DataTemplate), typeof(GraphicView));

        public static readonly DependencyProperty GraphicItemTemplateSelectorProperty =
            DependencyProperty.Register("GraphicItemTemplateSelector", typeof(DataTemplateSelector), typeof(GraphicView));

        public static readonly DependencyProperty GraphicItemContainerStyleProperty =
            DependencyProperty.Register("GraphicItemContainerStyle", typeof(Style), typeof(GraphicView));

        public static readonly RoutedEvent GraphicDragStartedEvent =
            EventManager.RegisterRoutedEvent("GraphicDragStarted", RoutingStrategy.Bubble, typeof(GraphicDragStartedEventHandler), typeof(GraphicView));

        public static readonly RoutedEvent GraphicDraggingEvent =
            EventManager.RegisterRoutedEvent("GraphicDragging", RoutingStrategy.Bubble, typeof(GraphicDraggingEventHandler), typeof(GraphicView));

        public static readonly RoutedEvent GraphicDragCompletedEvent =
            EventManager.RegisterRoutedEvent("GraphicDragCompleted", RoutingStrategy.Bubble, typeof(GraphicDragCompletedEventHandler), typeof(GraphicView));

        public static readonly RoutedCommand SelectAllCommand;
        public static readonly RoutedCommand SelectNoneCommand;
        public static readonly RoutedCommand InvertSelectionCommand;

        #endregion Dependency Property/Event Definitions

        /// <summary>
        /// Cached reference to the GraphicItemsControl in the visual-tree.
        /// </summary>
        private GraphicItemsControl graphicItemsControl;

        /// <summary>
        /// Cached list of currently selected graphics.
        /// </summary>
        private List<object> initialSelectedGraphics;

        /// <summary>
        /// Set to 'true' when the control key and the left mouse button is currently held down.
        /// </summary>
        private bool isControlAndLeftMouseButtonDown;

        /// <summary>
        /// Set to 'true' when the user is dragging out the selection rectangle.
        /// </summary>
        private bool isDraggingSelectionRect;

        /// <summary>
        /// Records the original mouse down point when the user is dragging out a selection rectangle.
        /// </summary>
        private Point origMouseDownPoint;

        /// <summary>
        /// A reference to the canvas that contains the drag selection rectangle.
        /// </summary>
        private FrameworkElement dragSelectionCanvas;

        /// <summary>
        /// The border that represents the drag selection rectangle.
        /// </summary>
        private FrameworkElement dragSelectionBorder;

        /// <summary>
        /// Cached list of selected GraphicItems, used while dragging graphics.
        /// </summary>
        private List<GraphicItem> cachedSelectedGraphicItems;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before drag-selection begins.
        /// </summary>
        private const double DragThreshold = 5;

        public GraphicView()
        {
            //
            // Create a collection to contain domains.
            //
            Graphics = new ObservableCollection<object>();

            //
            // Default background is white.
            //
            Background = Brushes.White;

            //
            // Add handlers for graphic drag events.
            //
            AddHandler(GraphicItem.GraphicDragStartedEvent, new GraphicDragStartedEventHandler(GraphicItem_DragStarted));
            AddHandler(GraphicItem.GraphicDraggingEvent, new GraphicDraggingEventHandler(GraphicItem_Dragging));
            AddHandler(GraphicItem.GraphicDragCompletedEvent, new GraphicDragCompletedEventHandler(GraphicItem_DragCompleted));
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static GraphicView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphicView), new FrameworkPropertyMetadata(typeof(GraphicView)));

            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.A, ModifierKeys.Control));
            SelectAllCommand = new RoutedCommand("SelectAll", typeof(GraphicView), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.Escape));
            SelectNoneCommand = new RoutedCommand("SelectNone", typeof(GraphicView), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.I, ModifierKeys.Control));
            InvertSelectionCommand = new RoutedCommand("InvertSelection", typeof(GraphicView), inputs);

            CommandBinding binding = new CommandBinding();
            binding.Command = SelectAllCommand;
            binding.Executed += SelectAll_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(GraphicView), binding);

            binding = new CommandBinding();
            binding.Command = SelectNoneCommand;
            binding.Executed += SelectNone_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(GraphicView), binding);

            binding = new CommandBinding();
            binding.Command = InvertSelectionCommand;
            binding.Executed += InvertSelection_Executed;
            CommandManager.RegisterClassCommandBinding(typeof(GraphicView), binding);
        }

        /// <summary>
        /// Event raised when the user starts dragging a graphic in the network.
        /// </summary>
        public event GraphicDragStartedEventHandler GraphicDragStarted
        {
            add { AddHandler(GraphicDragStartedEvent, value); }
            remove { RemoveHandler(GraphicDragStartedEvent, value); }
        }

        /// <summary>
        /// Event raised while user is dragging a graphic in the network.
        /// </summary>
        public event GraphicDraggingEventHandler GraphicDragging
        {
            add { AddHandler(GraphicDraggingEvent, value); }
            remove { RemoveHandler(GraphicDraggingEvent, value); }
        }

        /// <summary>
        /// Event raised when the user has completed dragging a graphic.
        /// </summary>
        public event GraphicDragCompletedEventHandler GraphicDragCompleted
        {
            add { AddHandler(GraphicDragCompletedEvent, value); }
            remove { RemoveHandler(GraphicDragCompletedEvent, value); }
        }

        /// <summary>
        /// Collection of domains in the model.
        /// </summary>
        public ObservableCollection<object> Graphics
        {
            get
            {
                return (ObservableCollection<object>)GetValue(GraphicsProperty);
            }
            private set
            {
                SetValue(GraphicsPropertyKey, value);
            }
        }

        /// <summary>
        /// A reference to the collection that is the source used to populate 'domains'.
        /// Used in the same way as 'ItemsSource' in 'ItemsControl'.
        /// </summary>
        public IEnumerable GraphicsSource
        {
            get
            {
                return (IEnumerable)GetValue(GraphicsSourceProperty);
            }
            set
            {
                SetValue(GraphicsSourceProperty, value);
            }
        }

        /// <summary>
        /// Set to 'true' to enable the clearing of selection when empty space is clicked.
        /// This is set to 'true' by default.
        /// </summary>
        public bool IsClearSelectionOnEmptySpaceClickEnabled
        {
            get
            {
                return (bool) GetValue(IsClearSelectionOnEmptySpaceClickEnabledProperty);
            }
            set
            {
                SetValue(IsClearSelectionOnEmptySpaceClickEnabledProperty, value);
            }
        }

        /// <summary>
        /// Set to 'true' to enable dragging of domains.
        /// </summary>
        public bool EnableGraphicDragging
        {
            get
            {
                return (bool)GetValue(EnableGraphicDraggingProperty);
            }
            set
            {
                SetValue(EnableGraphicDraggingProperty, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'true' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsDraggingGraphic
        {
            get
            {
                return (bool)GetValue(IsDraggingGraphicProperty);
            }
            private set
            {
                SetValue(IsDraggingGraphicPropertyKey, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'false' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsNotDraggingGraphic
        {
            get
            {
                return (bool)GetValue(IsNotDraggingGraphicProperty);
            }
            private set
            {
                SetValue(IsNotDraggingGraphicPropertyKey, value);
            }
        }

        /// <summary>
        /// Set to 'true' when the user is dragging either a graphic.
        /// </summary>
        public bool IsDragging
        {
            get
            {
                return (bool)GetValue(IsDraggingProperty);
            }
            private set
            {
                SetValue(IsDraggingPropertyKey, value);
            }
        }

        /// <summary>
        /// Set to 'true' when the user is not dragging anything.
        /// </summary>
        public bool IsNotDragging
        {
            get
            {
                return (bool)GetValue(IsNotDraggingProperty);
            }
            private set
            {
                SetValue(IsNotDraggingPropertyKey, value);
            }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display each graphic item.
        /// This is the equivalent to 'ItemTemplate' for ItemsControl.
        /// </summary>
        public DataTemplate GraphicItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(GraphicItemTemplateProperty);
            }
            set
            {
                SetValue(GraphicItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets custom style-selection logic for a style that can be applied to each generated container element. 
        /// This is the equivalent to 'ItemTemplateSelector' for ItemsControl.
        /// </summary>
        public DataTemplateSelector GraphicItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(GraphicItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(GraphicItemTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Style that is applied to the item container for each graphic item.
        /// This is the equivalent to 'ItemContainerStyle' for ItemsControl.
        /// </summary>
        public Style GraphicItemContainerStyle
        {
            get
            {
                return (Style)GetValue(GraphicItemContainerStyleProperty);
            }
            set
            {
                SetValue(GraphicItemContainerStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets the currently selected graphic.
        /// </summary>
        public object SelectedGraphic
        {
            get
            {
                if (graphicItemsControl != null)
                {
                    return graphicItemsControl.SelectedItem;
                }
                else
                {
                    if (initialSelectedGraphics == null)
                    {
                        return null;
                    }

                    if (initialSelectedGraphics.Count != 1)
                    {
                        return null;
                    }

                    return initialSelectedGraphics[0];
                }
            }
            set
            {
                if (graphicItemsControl != null)
                {
                    graphicItemsControl.SelectedItem = value;
                }
                else
                {
                    if (initialSelectedGraphics == null)
                    {
                        initialSelectedGraphics = new List<object>();
                    }

                    initialSelectedGraphics.Clear();
                    initialSelectedGraphics.Add(value);
                }
            }
        }

        /// <summary>
        /// Gets a list of selected graphics.
        /// </summary>
        public IList SelectedGraphics
        {
            get
            {
                if (graphicItemsControl != null)
                {
                    return graphicItemsControl.SelectedItems;
                }
                else
                {
                    if (initialSelectedGraphics == null)
                    {
                        initialSelectedGraphics = new List<object>();
                    }

                    return initialSelectedGraphics;
                }
            }
        }

        /// <summary>
        /// An event raised when the graphics selected in the view has changed.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Bring the currently selected graphics into view.
        /// This affects ContentViewportOffsetX/ContentViewportOffsetY, but doesn't affect 'ContentScale'.
        /// </summary>
        public void BringSelectedGraphicsIntoView()
        {
            BringGraphicsIntoView(SelectedGraphics);
        }

        /// <summary>
        /// Bring the collection of graphics into view.
        /// This affects ContentViewportOffsetX/ContentViewportOffsetY, but doesn't affect 'ContentScale'.
        /// </summary>
        public void BringGraphicsIntoView(ICollection graphics)
        {
            Contract.Requires<ArgumentNullException>(graphics != null);

            if (graphics.Count == 0)
            {
                return;
            }

            Rect rect = Rect.Empty;

            foreach (var graphic in graphics)
            {
                GraphicItem graphicItem = FindAssociatedGraphicItem(graphic);
                Rect graphicRect = new Rect(graphicItem.X, graphicItem.Y, graphicItem.ActualWidth, graphicItem.ActualHeight);

                if (rect == Rect.Empty)
                {
                    rect = graphicRect;
                }
                else
                {
                    rect.Intersect(graphicRect);
                }
            }

            BringIntoView(rect);
        }

        /// <summary>
        /// Clear the selection.
        /// </summary>
        public void SelectNone()
        {
            SelectedGraphics.Clear();
        }

        /// <summary>
        /// Selects all of the domains.
        /// </summary>
        public void SelectAll()
        {
            SelectAllGraphics();
        }

        private void SelectAllGraphics()
        {
            if (SelectedGraphics.Count == Graphics.Count) return;
            SelectedGraphics.Clear();
            foreach (var graphic in Graphics)
            {
                SelectedGraphics.Add(graphic);
            }
        }

        /// <summary>
        /// Inverts the current selection.
        /// </summary>
        public void InvertSelection()
        {
            var selectedGraphicsCopy = new ArrayList(SelectedGraphics);
            SelectedGraphics.Clear();

            foreach (var graphic in Graphics)
            {
                if (!selectedGraphicsCopy.Contains(graphic))
                {
                    SelectedGraphics.Add(graphic);
                }
            }
        }

        /// <summary>
        /// Executes the 'SelectAll' command.
        /// </summary>
        private static void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GraphicView c = (GraphicView)sender;
            c.SelectAll();
        }

        /// <summary>
        /// Executes the 'SelectNone' command.
        /// </summary>
        private static void SelectNone_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GraphicView c = (GraphicView)sender;
            c.SelectNone();
        }

        /// <summary>
        /// Executes the 'InvertSelection' command.
        /// </summary>
        private static void InvertSelection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GraphicView c = (GraphicView)sender;
            c.InvertSelection();
        }

        /// <summary>
        /// Called after the visual tree of the control has been built.
        /// Search for and cache references to named parts defined in the XAML control template for ModelView.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //
            // Cache the parts of the visual tree that we need access to later.
            //

            this.graphicItemsControl = (GraphicItemsControl)this.Template.FindName("PART_GraphicItemsControl", this);
            if (this.graphicItemsControl == null)
            {
                throw new ApplicationException("Failed to find 'PART_GraphicItemsControl' in the visual tree for 'ModelView'.");
            }

            //
            // Synchronize initial selected graphics to the GraphicItemsControl.
            //
            if (this.initialSelectedGraphics != null && this.initialSelectedGraphics.Any())
            {
                foreach (var graphic in this.initialSelectedGraphics)
                {
                    this.graphicItemsControl.SelectedItems.Add(graphic);
                }
            }

            this.initialSelectedGraphics = null; // Don't need this any more.

            this.graphicItemsControl.SelectionChanged += graphicItemsControl_SelectionChanged;

            this.dragSelectionCanvas = (FrameworkElement)Template.FindName("PART_DragSelectionCanvas", this);
            if (this.dragSelectionCanvas == null)
            {
                throw new ApplicationException("Failed to find 'PART_DragSelectionCanvas' in the visual tree for 'ModelView'.");
            }

            this.dragSelectionBorder = (FrameworkElement)Template.FindName("PART_DragSelectionBorder", this);
            if (this.dragSelectionBorder == null)
            {
                throw new ApplicationException("Failed to find 'PART_dragSelectionBorder' in the visual tree for 'ModelView'.");
            }
        }

        /// <summary>
        /// Event raised when a new collection has been assigned to the 'GraphicsSource' property.
        /// </summary>
        private static void GraphicsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GraphicView c = (GraphicView)d;

            //
            // Clear graphics.
            //
            c.Graphics.Clear();

            if (e.OldValue != null)
            {
                if (e.OldValue is INotifyCollectionChanged notifyCollectionChanged)
                {
                    //
                    // Unhook events from previous collection.
                    //
                    notifyCollectionChanged.CollectionChanged -= c.GraphicsSource_CollectionChanged;
                }
            }

            if (e.NewValue != null)
            {
                if (e.NewValue is IEnumerable enumerable)
                {
                    //
                    // Populate 'domains' from 'GraphicsSource'.
                    //
                    foreach (object obj in enumerable)
                    {
                        c.Graphics.Add(obj);
                    }
                }

                if (e.NewValue is INotifyCollectionChanged notifyCollectionChanged)
                {
                    //
                    // Hook events in new collection.
                    //
                    notifyCollectionChanged.CollectionChanged += c.GraphicsSource_CollectionChanged;
                }
            }
        }

        /// <summary>
        /// Event raised when a graphic has been added to or removed from the collection assigned to 'GraphicsSource'.
        /// </summary>
        private void GraphicsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Graphics.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (object obj in e.OldItems)
                    {
                        Graphics.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (object obj in e.NewItems)
                    {
                        Graphics.Add(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Find the max ZIndex of all the domains.
        /// </summary>
        internal int FindMaxZIndex()
        {
            if (this.graphicItemsControl == null)
            {
                return 0;
            }

            int maxZ = 0;

            for (int graphicIndex = 0; ; ++graphicIndex)
            {
                GraphicItem graphicItem = (GraphicItem)this.graphicItemsControl.ItemContainerGenerator.ContainerFromIndex(graphicIndex);
                if (graphicItem == null)
                {
                    break;
                }

                if (graphicItem.ZIndex > maxZ)
                {
                    maxZ = graphicItem.ZIndex;
                }
            }

            return maxZ;
        }

        /// <summary>
        /// Find the GraphicItem UI element that is associated with graphic.
        /// </summary>
        internal GraphicItem FindAssociatedGraphicItem(object graphic)
        {
            if (!(graphic is GraphicItem graphicItem))
            {
                graphicItem = graphicItemsControl.FindAssociatedGraphicItem(graphic);
            }
            return graphicItem;
        }

        /// <summary>
        /// De-select all graphics.
        /// </summary>
        internal void DeselectAll()
        {
            SelectedGraphics.Clear();
        }

        /// <summary>
        /// Called when the user holds down the mouse.
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Focus();

            if (e.ChangedButton == MouseButton.Left &&
                (Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                DeselectAll();

                isControlAndLeftMouseButtonDown = true;
                origMouseDownPoint = e.GetPosition(this);

                CaptureMouse();

                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when the user releases the mouse.
        /// </summary>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.ChangedButton == MouseButton.Left)
            {
                bool wasDragSelectionApplied = false;

                if (isDraggingSelectionRect)
                {
                    //
                    // Drag selection has ended, apply the 'selection rectangle'.
                    //

                    isDraggingSelectionRect = false;
                    ApplyDragSelectionRect();

                    e.Handled = true;
                    wasDragSelectionApplied = true;
                }

                if (isControlAndLeftMouseButtonDown)
                {
                    isControlAndLeftMouseButtonDown = false;
                    ReleaseMouseCapture();


                    e.Handled = true;
                }

                if (!wasDragSelectionApplied && IsClearSelectionOnEmptySpaceClickEnabled)
                {
                    //
                    // A click and release in empty space clears the selection.
                    //
                    DeselectAll();
                }
            }
        }

        /// <summary>
        /// Called when the user moves the mouse.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDraggingSelectionRect)
            {
                //
                // Drag selection is in progress.
                //
                Point curMouseDownPoint = e.GetPosition(this);
                UpdateDragSelectionRect(origMouseDownPoint, curMouseDownPoint);

                e.Handled = true;
            }
            else if (isControlAndLeftMouseButtonDown)
            {
                //
                // The user is left-dragging the mouse,
                // but don't initiate drag selection until
                // they have dragged past the threshold value.
                //
                Point curMouseDownPoint = e.GetPosition(this);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance > DragThreshold)
                {
                    //
                    // When the mouse has been dragged more than the threshold value commence drag selection.
                    //
                    isDraggingSelectionRect = true;
                    InitDragSelectionRect(origMouseDownPoint, curMouseDownPoint);
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// Initialize the rectangle used for drag selection.
        /// </summary>
        private void InitDragSelectionRect(Point pt1, Point pt2)
        {
            UpdateDragSelectionRect(pt1, pt2);

            dragSelectionCanvas.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Update the position and size of the rectangle used for drag selection.
        /// </summary>
        private void UpdateDragSelectionRect(Point pt1, Point pt2)
        {
            double x, y, width, height;

            //
            // Determine x,y,width and height of the rect inverting the points if necessary.
            // 

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            //
            // Update the coordinates of the rectangle used for drag selection.
            //
            Canvas.SetLeft(dragSelectionBorder, x);
            Canvas.SetTop(dragSelectionBorder, y);
            dragSelectionBorder.Width = width;
            dragSelectionBorder.Height = height;
        }

        /// <summary>
        /// Select all graphics that are in the drag selection rectangle.
        /// </summary>
        private void ApplyDragSelectionRect()
        {
            dragSelectionCanvas.Visibility = Visibility.Collapsed;

            double x = Canvas.GetLeft(dragSelectionBorder);
            double y = Canvas.GetTop(dragSelectionBorder);
            double width = dragSelectionBorder.Width;
            double height = dragSelectionBorder.Height;
            Rect dragRect = new Rect(x, y, width, height);

            //
            // Inflate the drag selection-rectangle by 1/10 of its size to 
            // make sure the intended item is selected.
            //
            dragRect.Inflate(width / 10, height / 10);

            //
            // Clear the current selection.
            //
            this.graphicItemsControl.SelectedItems.Clear();

            //
            // Find and select all the graphic list box items.
            //
            for (int graphicIndex = 0; graphicIndex < Graphics.Count; ++graphicIndex)
            {
                var graphicItem = (GraphicItem)this.graphicItemsControl.ItemContainerGenerator.ContainerFromIndex(graphicIndex);
                var transformToAncestor = graphicItem.TransformToAncestor(this);
                var itemPt1 = transformToAncestor.Transform(new Point(0, 0));
                var itemPt2 = transformToAncestor.Transform(new Point(graphicItem.ActualWidth, graphicItem.ActualHeight));
                var itemRect = new Rect(itemPt1, itemPt2);
                if (dragRect.Contains(itemRect))
                {
                    graphicItem.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// Event raised when the selection in GraphicItemsControl changes.
        /// </summary>
        private void graphicItemsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(ListBox.SelectionChangedEvent, e.RemovedItems, e.AddedItems));
        }

        /// <summary>
        /// Event raised when the user starts to drag a graphic.
        /// </summary>
        private void GraphicItem_DragStarted(object source, GraphicDragStartedEventArgs e)
        {
            e.Handled = true;

            IsDragging = true;
            IsNotDragging = false;
            IsDraggingGraphic = true;
            IsNotDraggingGraphic = false;

            RaiseEvent(new GraphicDragStartedEventArgs(GraphicDragStartedEvent, this, SelectedGraphics));

            e.Cancel = new GraphicDragStartedEventArgs(GraphicDragStartedEvent, this, SelectedGraphics).Cancel;
        }

        /// <summary>
        /// Event raised while the user is dragging a graphic.
        /// </summary>
        private void GraphicItem_Dragging(object source, GraphicDraggingEventArgs e)
        {
            e.Handled = true;

            //
            // Cache the GraphicItem for each selected graphic whilst dragging is in progress.
            //
            if (this.cachedSelectedGraphicItems == null)
            {
                this.cachedSelectedGraphicItems = new List<GraphicItem>();

                foreach (var selectedGraphic in SelectedGraphics)
                {
                    var graphicItem = FindAssociatedGraphicItem(selectedGraphic);
                    if (graphicItem == null)
                    {
                        throw new ApplicationException("Unexpected code path!");
                    }

                    this.cachedSelectedGraphicItems.Add(graphicItem);
                }
            }

            // 
            // Update the position of the graphic within the Canvas.
            //
            foreach (var graphicItem in this.cachedSelectedGraphicItems)
            {
                graphicItem.X += e.HorizontalChange;
                graphicItem.Y += e.VerticalChange;
            }

            RaiseEvent(new GraphicDraggingEventArgs(GraphicDraggingEvent, this, SelectedGraphics, e.HorizontalChange, e.VerticalChange));
        }

        /// <summary>
        /// Event raised when the user has finished dragging a graphic.
        /// </summary>
        private void GraphicItem_DragCompleted(object source, GraphicDragCompletedEventArgs e)
        {
            e.Handled = true;

            RaiseEvent(new GraphicDragCompletedEventArgs(GraphicDragCompletedEvent, this, SelectedGraphics));

            cachedSelectedGraphicItems = null;

            IsDragging = false;
            IsNotDragging = true;
            IsDraggingGraphic = false;
            IsNotDraggingGraphic = true;
        }
    }
}
