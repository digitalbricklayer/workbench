using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DynaApp.Controls;
using DynaApp.Events;
using DynaApp.ViewModels;

namespace DynaApp.Views
{
    /// <summary>
    /// Model view graphically displays the model.
    /// </summary>
    public class ModelView : Control
    {
        #region Dependency Property/Event Definitions

        private static readonly DependencyPropertyKey VariablesPropertyKey =
            DependencyProperty.RegisterReadOnly("Variables", typeof(ObservableCollection<object>), typeof(ModelView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty VariablesProperty = VariablesPropertyKey.DependencyProperty;

        public static readonly DependencyProperty VariablesSourceProperty =
            DependencyProperty.Register("VariablesSource", typeof(IEnumerable), typeof(ModelView),
                new FrameworkPropertyMetadata(VariablesSource_PropertyChanged));

        public static readonly DependencyProperty DomainsSourceProperty =
            DependencyProperty.Register("DomainsSource", typeof(IEnumerable), typeof(ModelView),
                new FrameworkPropertyMetadata(VariablesSource_PropertyChanged));

        public static readonly DependencyProperty ConstraintsSourceProperty =
            DependencyProperty.Register("ConstraintsSource", typeof(IEnumerable), typeof(ModelView),
                new FrameworkPropertyMetadata(VariablesSource_PropertyChanged));

        public static readonly DependencyProperty IsClearSelectionOnEmptySpaceClickEnabledProperty =
            DependencyProperty.Register("IsClearSelectionOnEmptySpaceClickEnabled", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty EnableVariableDraggingProperty =
            DependencyProperty.Register("EnableVariableDragging", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(true));

        private static readonly DependencyPropertyKey IsDraggingVariablePropertyKey =
            DependencyProperty.RegisterReadOnly("IsDraggingVariable", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsDraggingVariableProperty = IsDraggingVariablePropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNotDraggingVariablePropertyKey =
            DependencyProperty.RegisterReadOnly("IsNotDraggingVariable", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsNotDraggingVariableProperty = IsDraggingVariablePropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsDraggingPropertyKey =
            DependencyProperty.RegisterReadOnly("IsDragging", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNotDraggingPropertyKey =
            DependencyProperty.RegisterReadOnly("IsNotDragging", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsNotDraggingProperty = IsNotDraggingPropertyKey.DependencyProperty;

        public static readonly DependencyProperty GraphicItemTemplateProperty =
            DependencyProperty.Register("GraphicItemTemplate", typeof(DataTemplate), typeof(ModelView));

        public static readonly DependencyProperty GraphicItemTemplateSelectorProperty =
            DependencyProperty.Register("GraphicItemTemplateSelector", typeof(DataTemplateSelector), typeof(ModelView));

        public static readonly DependencyProperty GraphicItemContainerStyleProperty =
            DependencyProperty.Register("GraphicItemContainerStyle", typeof(Style), typeof(ModelView));

        public static readonly DependencyProperty DomainItemTemplateProperty =
            DependencyProperty.Register("DomainItemTemplate", typeof(DataTemplate), typeof(ModelView));

        public static readonly DependencyProperty DomainItemTemplateSelectorProperty =
            DependencyProperty.Register("DomainItemTemplateSelector", typeof(DataTemplateSelector), typeof(ModelView));

        public static readonly DependencyProperty DomainItemContainerStyleProperty =
            DependencyProperty.Register("DomainItemContainerStyle", typeof(Style), typeof(ModelView));

        public static readonly DependencyProperty ConstraintItemTemplateProperty =
            DependencyProperty.Register("ConstraintItemTemplate", typeof(DataTemplate), typeof(ModelView));

        public static readonly DependencyProperty ConstraintItemTemplateSelectorProperty =
            DependencyProperty.Register("ConstraintItemTemplateSelector", typeof(DataTemplateSelector), typeof(ModelView));

        public static readonly DependencyProperty ConstraintItemContainerStyleProperty =
            DependencyProperty.Register("ConstraintItemContainerStyle", typeof(Style), typeof(ModelView));

        public static readonly RoutedEvent VariableDragStartedEvent =
            EventManager.RegisterRoutedEvent("VariableDragStarted", RoutingStrategy.Bubble, typeof(VariableDragStartedEventHandler), typeof(ModelView));

        public static readonly RoutedEvent VariableDraggingEvent =
            EventManager.RegisterRoutedEvent("VariableDragging", RoutingStrategy.Bubble, typeof(VariableDraggingEventHandler), typeof(ModelView));

        public static readonly RoutedEvent VariableDragCompletedEvent =
            EventManager.RegisterRoutedEvent("VariableDragCompleted", RoutingStrategy.Bubble, typeof(VariableDragCompletedEventHandler), typeof(ModelView));

        public static readonly RoutedCommand SelectAllCommand;
        public static readonly RoutedCommand SelectNoneCommand;
        public static readonly RoutedCommand InvertSelectionCommand;

        #endregion Dependency Property/Event Definitions

        /// <summary>
        /// Cached reference to the VariableItemsControl in the visual-tree.
        /// </summary>
        private GraphicItemsControl graphicItemsControl;

        /// <summary>
        /// Cached list of currently selected Variables.
        /// </summary>
        private List<object> initialSelectedVariables;

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
        /// Cached list of selected VariableItems, used while dragging domains.
        /// </summary>
        private List<GraphicItem> cachedSelectedVariableItems;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before drag-selection begins.
        /// </summary>
        private const double DragThreshold = 5;

        public ModelView()
        {
            //
            // Create a collection to contain domains.
            //
            this.Variables = new ObservableCollection<object>();

            //
            // Default background is white.
            //
            this.Background = Brushes.White;

            //
            // Add handlers for graphic drag events.
            //
            AddHandler(GraphicItem.VariableDragStartedEvent, new VariableDragStartedEventHandler(VariableItem_DragStarted));
            AddHandler(GraphicItem.VariableDraggingEvent, new VariableDraggingEventHandler(VariableItem_Dragging));
            AddHandler(GraphicItem.VariableDragCompletedEvent, new VariableDragCompletedEventHandler(VariableItem_DragCompleted));
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ModelView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModelView), new FrameworkPropertyMetadata(typeof(ModelView)));

            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.A, ModifierKeys.Control));
            SelectAllCommand = new RoutedCommand("SelectAll", typeof(ModelView), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.Escape));
            SelectNoneCommand = new RoutedCommand("SelectNone", typeof(ModelView), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.I, ModifierKeys.Control));
            InvertSelectionCommand = new RoutedCommand("InvertSelection", typeof(ModelView), inputs);

            CommandBinding binding = new CommandBinding();
            binding.Command = SelectAllCommand;
            binding.Executed += new ExecutedRoutedEventHandler(SelectAll_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(ModelView), binding);

            binding = new CommandBinding();
            binding.Command = SelectNoneCommand;
            binding.Executed += new ExecutedRoutedEventHandler(SelectNone_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(ModelView), binding);

            binding = new CommandBinding();
            binding.Command = InvertSelectionCommand;
            binding.Executed += new ExecutedRoutedEventHandler(InvertSelection_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(ModelView), binding);
        }

        /// <summary>
        /// Gets the model view model.
        /// </summary>
        public ModelViewModel ViewModel
        {
            get
            {
                return (ModelViewModel)this.DataContext;
            }
        }

        /// <summary>
        /// Event raised when the user starts dragging a variable in the network.
        /// </summary>
        public event VariableDragStartedEventHandler VariableDragStarted
        {
            add { AddHandler(VariableDragStartedEvent, value); }
            remove { RemoveHandler(VariableDragStartedEvent, value); }
        }

        /// <summary>
        /// Event raised while user is dragging a variable in the network.
        /// </summary>
        public event VariableDraggingEventHandler VariableDragging
        {
            add { AddHandler(VariableDraggingEvent, value); }
            remove { RemoveHandler(VariableDraggingEvent, value); }
        }

        /// <summary>
        /// Event raised when the user has completed dragging a variable in the network.
        /// </summary>
        public event VariableDragCompletedEventHandler VariableDragCompleted
        {
            add { AddHandler(VariableDragCompletedEvent, value); }
            remove { RemoveHandler(VariableDragCompletedEvent, value); }
        }

        /// <summary>
        /// Collection of domains in the model.
        /// </summary>
        public ObservableCollection<object> Variables
        {
            get
            {
                return (ObservableCollection<object>)GetValue(VariablesProperty);
            }
            private set
            {
                SetValue(VariablesPropertyKey, value);
            }
        }

        /// <summary>
        /// A reference to the collection that is the source used to populate 'domains'.
        /// Used in the same way as 'ItemsSource' in 'ItemsControl'.
        /// </summary>
        public IEnumerable VariablesSource
        {
            get
            {
                return (IEnumerable)GetValue(VariablesSourceProperty);
            }
            set
            {
                SetValue(VariablesSourceProperty, value);
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
        public bool EnableVariableDragging
        {
            get
            {
                return (bool)GetValue(EnableVariableDraggingProperty);
            }
            set
            {
                SetValue(EnableVariableDraggingProperty, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'true' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsDraggingVariable
        {
            get
            {
                return (bool)GetValue(IsDraggingVariableProperty);
            }
            private set
            {
                SetValue(IsDraggingVariablePropertyKey, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'false' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsNotDraggingVariable
        {
            get
            {
                return (bool)GetValue(IsNotDraggingVariableProperty);
            }
            private set
            {
                SetValue(IsNotDraggingVariablePropertyKey, value);
            }
        }

        /// <summary>
        /// Set to 'true' when the user is dragging either a variable or a connection.
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
        /// Gets or sets the DataTemplate used to display each variable item.
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
        /// Gets or sets the Style that is applied to the item container for each variable item.
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
        /// A reference to currently selected variable.
        /// </summary>
        public object SelectedVariable
        {
            get
            {
                if (graphicItemsControl != null)
                {
                    return graphicItemsControl.SelectedItem;
                }
                else
                {
                    if (initialSelectedVariables == null)
                    {
                        return null;
                    }

                    if (initialSelectedVariables.Count != 1)
                    {
                        return null;
                    }

                    return initialSelectedVariables[0];
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
                    if (initialSelectedVariables == null)
                    {
                        initialSelectedVariables = new List<object>();
                    }

                    initialSelectedVariables.Clear();
                    initialSelectedVariables.Add(value);
                }
            }
        }

        /// <summary>
        /// A list of selected domains.
        /// </summary>
        public IList SelectedVariables
        {
            get
            {
                if (graphicItemsControl != null)
                {
                    return graphicItemsControl.SelectedItems;
                }
                else
                {
                    if (initialSelectedVariables == null)
                    {
                        initialSelectedVariables = new List<object>();
                    }

                    return initialSelectedVariables;
                }
            }
        }

        /// <summary>
        /// An event raised when the domains selected in the NetworkView has changed.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Bring the currently selected domains into view.
        /// This affects ContentViewportOffsetX/ContentViewportOffsetY, but doesn't affect 'ContentScale'.
        /// </summary>
        public void BringSelectedVariablesIntoView()
        {
            BringVariablesIntoView(SelectedVariables);
        }

        /// <summary>
        /// Bring the collection of domains into view.
        /// This affects ContentViewportOffsetX/ContentViewportOffsetY, but doesn't affect 'ContentScale'.
        /// </summary>
        public void BringVariablesIntoView(ICollection variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException("variables");
            }

            if (variables.Count == 0)
            {
                return;
            }

            Rect rect = Rect.Empty;

            foreach (var variable in variables)
            {
                GraphicItem graphicItem = FindAssociatedVariableItem(variable);
                Rect variableRect = new Rect(graphicItem.X, graphicItem.Y, graphicItem.ActualWidth, graphicItem.ActualHeight);

                if (rect == Rect.Empty)
                {
                    rect = variableRect;
                }
                else
                {
                    rect.Intersect(variableRect);
                }
            }

            this.BringIntoView(rect);
        }

        /// <summary>
        /// Clear the selection.
        /// </summary>
        public void SelectNone()
        {
            this.SelectedVariables.Clear();
        }

        /// <summary>
        /// Selects all of the domains.
        /// </summary>
        public void SelectAll()
        {
            this.SelectAllVariables();
        }

        private void SelectAllVariables()
        {
            if (this.SelectedVariables.Count == this.Variables.Count) return;
            this.SelectedVariables.Clear();
            foreach (var variable in this.Variables)
            {
                this.SelectedVariables.Add(variable);
            }
        }

        /// <summary>
        /// Inverts the current selection.
        /// </summary>
        public void InvertSelection()
        {
            var selectedVariablesCopy = new ArrayList(this.SelectedVariables);
            this.SelectedVariables.Clear();

            foreach (var variable in this.Variables)
            {
                if (!selectedVariablesCopy.Contains(variable))
                {
                    this.SelectedVariables.Add(variable);
                }
            }
        }

        private HitTestResult HitTestContainers(Point hitPoint)
        {
            HitTestResult result = null;
            VisualTreeHelper.HitTest(graphicItemsControl, null,
                //
                // Result callback delegate.
                // This method is called when we have a result.
                //
                delegate(HitTestResult hitTestResult)
                {
                    result = hitTestResult;

                    return HitTestResultBehavior.Stop;
                },
                new PointHitTestParameters(hitPoint));

            return result;
        }

        /// <summary>
        /// Executes the 'SelectAll' command.
        /// </summary>
        private static void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ModelView c = (ModelView)sender;
            c.SelectAll();
        }

        /// <summary>
        /// Executes the 'SelectNone' command.
        /// </summary>
        private static void SelectNone_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ModelView c = (ModelView)sender;
            c.SelectNone();
        }

        /// <summary>
        /// Executes the 'InvertSelection' command.
        /// </summary>
        private static void InvertSelection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ModelView c = (ModelView)sender;
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

            this.graphicItemsControl = (GraphicItemsControl)this.Template.FindName("PART_VariableItemsControl", this);
            if (this.graphicItemsControl == null)
            {
                throw new ApplicationException("Failed to find 'PART_VariableItemsControl' in the visual tree for 'ModelView'.");
            }

            //
            // Synchronize initial selected variables to the VariableItemsControl.
            //
            if (this.initialSelectedVariables != null && this.initialSelectedVariables.Any())
            {
                foreach (var variable in this.initialSelectedVariables)
                {
                    this.graphicItemsControl.SelectedItems.Add(variable);
                }
            }

            this.initialSelectedVariables = null; // Don't need this any more.

            this.graphicItemsControl.SelectionChanged += new SelectionChangedEventHandler(variableItemsControl_SelectionChanged);

            this.dragSelectionCanvas = (FrameworkElement)this.Template.FindName("PART_DragSelectionCanvas", this);
            if (this.dragSelectionCanvas == null)
            {
                throw new ApplicationException("Failed to find 'PART_DragSelectionCanvas' in the visual tree for 'ModelView'.");
            }

            this.dragSelectionBorder = (FrameworkElement)this.Template.FindName("PART_DragSelectionBorder", this);
            if (this.dragSelectionBorder == null)
            {
                throw new ApplicationException("Failed to find 'PART_dragSelectionBorder' in the visual tree for 'ModelView'.");
            }
        }

        /// <summary>
        /// Event raised when a new collection has been assigned to the 'VariablesSource' property.
        /// </summary>
        private static void VariablesSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ModelView c = (ModelView)d;

            //
            // Clear variables.
            //
            c.Variables.Clear();

            if (e.OldValue != null)
            {
                var notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Unhook events from previous collection.
                    //
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.VariablesSource_CollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                var enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    //
                    // Populate 'domains' from 'VariablesSource'.
                    //
                    foreach (object obj in enumerable)
                    {
                        c.Variables.Add(obj);
                    }
                }

                var notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Hook events in new collection.
                    //
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.VariablesSource_CollectionChanged);
                }
            }
        }

        /// <summary>
        /// Event raised when a variable has been added to or removed from the collection assigned to 'VariablesSource'.
        /// </summary>
        private void VariablesSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //
                // 'VariablesSource' has been cleared, also clear 'domains'.
                //
                this.Variables.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {
                    //
                    // For each item that has been removed from 'VariablesSource' also remove it from 'domains'.
                    //
                    foreach (object obj in e.OldItems)
                    {
                        this.Variables.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {
                    //
                    // For each item that has been added to 'VariablesSource' also add it to 'domains'.
                    //
                    foreach (object obj in e.NewItems)
                    {
                        this.Variables.Add(obj);
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

            for (int variableIndex = 0; ; ++variableIndex)
            {
                GraphicItem graphicItem = (GraphicItem)this.graphicItemsControl.ItemContainerGenerator.ContainerFromIndex(variableIndex);
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
        /// Find the VariableItem UI element that is associated with 'variable'.
        /// 'variable' can be a view-model object, in which case the visual-tree
        /// is searched for the associated VariableItem.
        /// Otherwise 'variable' can actually be a 'VariableItem' in which case it is 
        /// simply returned.
        /// </summary>
        internal GraphicItem FindAssociatedVariableItem(object variable)
        {
            GraphicItem graphicItem = variable as GraphicItem;
            if (graphicItem == null)
            {
                graphicItem = graphicItemsControl.FindAssociatedVariableItem(variable);
            }
            return graphicItem;
        }

        /// <summary>
        /// De-select all graphics.
        /// </summary>
        internal void DeselectAll()
        {
            this.SelectedVariables.Clear();
        }

        /// <summary>
        /// Called when the user holds down the mouse.
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            this.Focus();

            if (e.ChangedButton == MouseButton.Left &&
                (Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                this.DeselectAll();

                isControlAndLeftMouseButtonDown = true;
                origMouseDownPoint = e.GetPosition(this);

                this.CaptureMouse();

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
                    this.ReleaseMouseCapture();


                    e.Handled = true;
                }

                if (!wasDragSelectionApplied && IsClearSelectionOnEmptySpaceClickEnabled)
                {
                    //
                    // A click and release in empty space clears the selection.
                    //
                    this.DeselectAll();
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
            // Find and select all the variable list box items.
            //
            for (int variableIndex = 0; variableIndex < this.Variables.Count; ++variableIndex)
            {
                var variableItem = (GraphicItem)this.graphicItemsControl.ItemContainerGenerator.ContainerFromIndex(variableIndex);
                var transformToAncestor = variableItem.TransformToAncestor(this);
                Point itemPt1 = transformToAncestor.Transform(new Point(0, 0));
                Point itemPt2 = transformToAncestor.Transform(new Point(variableItem.ActualWidth, variableItem.ActualHeight));
                Rect itemRect = new Rect(itemPt1, itemPt2);
                if (dragRect.Contains(itemRect))
                {
                    variableItem.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// Event raised when the selection in 'variableItemsControl' changes.
        /// </summary>
        private void variableItemsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, new SelectionChangedEventArgs(ListBox.SelectionChangedEvent, e.RemovedItems, e.AddedItems));
            }
        }

        /// <summary>
        /// Event raised when the user starts to drag a variable.
        /// </summary>
        private void VariableItem_DragStarted(object source, VariableDragStartedEventArgs e)
        {
            e.Handled = true;

            this.IsDragging = true;
            this.IsNotDragging = false;
            this.IsDraggingVariable = true;
            this.IsNotDraggingVariable = false;

            var eventArgs = new VariableDragStartedEventArgs(VariableDragStartedEvent, this, this.SelectedVariables);            
            RaiseEvent(eventArgs);

            e.Cancel = eventArgs.Cancel;
        }

        /// <summary>
        /// Event raised while the user is dragging a variable.
        /// </summary>
        private void VariableItem_Dragging(object source, VariableDraggingEventArgs e)
        {
            e.Handled = true;

            //
            // Cache the VariableItem for each selected variable whilst dragging is in progress.
            //
            if (this.cachedSelectedVariableItems == null)
            {
                this.cachedSelectedVariableItems = new List<GraphicItem>();

                foreach (var selectedVariable in this.SelectedVariables)
                {
                    var variableItem = FindAssociatedVariableItem(selectedVariable);
                    if (variableItem == null)
                    {
                        throw new ApplicationException("Unexpected code path!");
                    }

                    this.cachedSelectedVariableItems.Add(variableItem);
                }
            }

            // 
            // Update the position of the variable within the Canvas.
            //
            foreach (var variableItem in this.cachedSelectedVariableItems)
            {
                variableItem.X += e.HorizontalChange;
                variableItem.Y += e.VerticalChange;
            }

            var eventArgs = new VariableDraggingEventArgs(VariableDraggingEvent, this, this.SelectedVariables, e.HorizontalChange, e.VerticalChange);
            RaiseEvent(eventArgs);
        }

        /// <summary>
        /// Event raised when the user has finished dragging a variable.
        /// </summary>
        private void VariableItem_DragCompleted(object source, VariableDragCompletedEventArgs e)
        {
            e.Handled = true;

            var eventArgs = new VariableDragCompletedEventArgs(VariableDragCompletedEvent, this, this.SelectedVariables);
            RaiseEvent(eventArgs);

            if (cachedSelectedVariableItems != null)
            {
                cachedSelectedVariableItems = null;
            }

            this.IsDragging = false;
            this.IsNotDragging = true;
            this.IsDraggingVariable = false;
            this.IsNotDraggingVariable = true;
        }
    }
}
