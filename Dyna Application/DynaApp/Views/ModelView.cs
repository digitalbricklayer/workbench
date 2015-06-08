using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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

        private static readonly DependencyPropertyKey DomainsPropertyKey =
            DependencyProperty.RegisterReadOnly("Domains", typeof(ObservableCollection<object>), typeof(ModelView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty DomainsProperty = DomainsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ConnectionsPropertyKey =
            DependencyProperty.RegisterReadOnly("Connections", typeof(ObservableCollection<object>), typeof(ModelView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ConnectionsProperty = ConnectionsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty VariablesSourceProperty =
            DependencyProperty.Register("VariablesSource", typeof(IEnumerable), typeof(ModelView),
                new FrameworkPropertyMetadata(VariablesSource_PropertyChanged));

        public static readonly DependencyProperty DomainsSourceProperty =
            DependencyProperty.Register("DomainsSource", typeof(IEnumerable), typeof(ModelView),
                new FrameworkPropertyMetadata(DomainsSource_PropertyChanged));

        public static readonly DependencyProperty ConnectionsSourceProperty =
            DependencyProperty.Register("ConnectionsSource", typeof(IEnumerable), typeof(ModelView),
                new FrameworkPropertyMetadata(ConnectionsSource_PropertyChanged));

        public static readonly DependencyProperty IsClearSelectionOnEmptySpaceClickEnabledProperty =
            DependencyProperty.Register("IsClearSelectionOnEmptySpaceClickEnabled", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty EnableConnectionDraggingProperty =
            DependencyProperty.Register("EnableConnectionDragging", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(true));

        private static readonly DependencyPropertyKey IsDraggingConnectionPropertyKey =
            DependencyProperty.RegisterReadOnly("IsDraggingConnection", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsDraggingConnectionProperty = IsDraggingConnectionPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNotDraggingConnectionPropertyKey =
            DependencyProperty.RegisterReadOnly("IsNotDraggingConnection", typeof(bool), typeof(ModelView),
                new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsNotDraggingConnectionProperty = IsNotDraggingConnectionPropertyKey.DependencyProperty;

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

        public static readonly DependencyProperty VariableItemTemplateProperty =
            DependencyProperty.Register("VariableItemTemplate", typeof(DataTemplate), typeof(ModelView));

        public static readonly DependencyProperty VariableItemTemplateSelectorProperty =
            DependencyProperty.Register("VariableItemTemplateSelector", typeof(DataTemplateSelector), typeof(ModelView));

        public static readonly DependencyProperty VariableItemContainerStyleProperty =
            DependencyProperty.Register("VariableItemContainerStyle", typeof(Style), typeof(ModelView));

        public static readonly DependencyProperty DomainItemTemplateProperty =
            DependencyProperty.Register("DomainItemTemplate", typeof(DataTemplate), typeof(ModelView));

        public static readonly DependencyProperty DomainItemTemplateSelectorProperty =
            DependencyProperty.Register("DomainItemTemplateSelector", typeof(DataTemplateSelector), typeof(ModelView));

        public static readonly DependencyProperty DomainItemContainerStyleProperty =
            DependencyProperty.Register("DomainItemContainerStyle", typeof(Style), typeof(ModelView));

        public static readonly DependencyProperty ConnectionItemTemplateProperty =
            DependencyProperty.Register("ConnectionItemTemplate", typeof(DataTemplate), typeof(ModelView));

        public static readonly DependencyProperty ConnectionItemTemplateSelectorProperty =
            DependencyProperty.Register("ConnectionItemTemplateSelector", typeof(DataTemplateSelector), typeof(ModelView));

        public static readonly DependencyProperty ConnectionItemContainerStyleProperty =
            DependencyProperty.Register("ConnectionItemContainerStyle", typeof(Style), typeof(ModelView));

        public static readonly RoutedEvent VariableDragStartedEvent =
            EventManager.RegisterRoutedEvent("VariableDragStarted", RoutingStrategy.Bubble, typeof(VariableDragStartedEventHandler), typeof(ModelView));

        public static readonly RoutedEvent VariableDraggingEvent =
            EventManager.RegisterRoutedEvent("VariableDragging", RoutingStrategy.Bubble, typeof(VariableDraggingEventHandler), typeof(ModelView));

        public static readonly RoutedEvent VariableDragCompletedEvent =
            EventManager.RegisterRoutedEvent("VariableDragCompleted", RoutingStrategy.Bubble, typeof(VariableDragCompletedEventHandler), typeof(ModelView));

        public static readonly RoutedEvent DomainDragStartedEvent =
            EventManager.RegisterRoutedEvent("DomainDragStarted", RoutingStrategy.Bubble, typeof(DomainDragStartedEventHandler), typeof(ModelView));

        public static readonly RoutedEvent DomainDraggingEvent =
            EventManager.RegisterRoutedEvent("DomainDragging", RoutingStrategy.Bubble, typeof(DomainDraggingEventHandler), typeof(ModelView));

        public static readonly RoutedEvent DomainDragCompletedEvent =
            EventManager.RegisterRoutedEvent("DomainDragCompleted", RoutingStrategy.Bubble, typeof(DomainDragCompletedEventHandler), typeof(ModelView));

        public static readonly RoutedEvent ConnectionDragStartedEvent =
            EventManager.RegisterRoutedEvent("ConnectionDragStarted", RoutingStrategy.Bubble, typeof(ConnectionDragStartedEventHandler), typeof(ModelView));

        public static readonly RoutedEvent QueryConnectionFeedbackEvent =
            EventManager.RegisterRoutedEvent("QueryConnectionFeedback", RoutingStrategy.Bubble, typeof(QueryConnectionFeedbackEventHandler), typeof(ModelView));

        public static readonly RoutedEvent ConnectionDraggingEvent =
            EventManager.RegisterRoutedEvent("ConnectionDragging", RoutingStrategy.Bubble, typeof(ConnectionDraggingEventHandler), typeof(ModelView));

        public static readonly RoutedEvent ConnectionDragCompletedEvent =
            EventManager.RegisterRoutedEvent("ConnectionDragCompleted", RoutingStrategy.Bubble, typeof(ConnectionDragCompletedEventHandler), typeof(ModelView));

        public static readonly RoutedCommand SelectAllCommand = null;
        public static readonly RoutedCommand SelectNoneCommand = null;
        public static readonly RoutedCommand InvertSelectionCommand = null;
        public static readonly RoutedCommand CancelConnectionDraggingCommand = null;

        #endregion Dependency Property/Event Definitions

        /// <summary>
        /// Cached reference to the VariableItemsControl in the visual-tree.
        /// </summary>
        private VariableItemsControl variableItemsControl;

        /// <summary>
        /// Cached reference to the DomainItemsControl in the visual-tree.
        /// </summary>
        private DomainItemsControl domainItemsControl;

        /// <summary>
        /// Cached reference to the ItemsControl for connections in the visual-tree.
        /// </summary>
        private ItemsControl connectionItemsControl;

        /// <summary>
        /// Cached list of currently selected variables.
        /// </summary>
        private List<object> initialSelectedVariables;

        /// <summary>
        /// Cached list of currently selected domains.
        /// </summary>
        private List<object> initialSelectedDomains;

        /// <summary>
        /// Set to 'true' when the control key and the left mouse button is currently held down.
        /// </summary>
        private bool isControlAndLeftMouseButtonDown = false;

        /// <summary>
        /// Set to 'true' when the user is dragging out the selection rectangle.
        /// </summary>
        private bool isDraggingSelectionRect = false;

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
        private List<VariableItem> cachedSelectedVariableItems;

        /// <summary>
        /// Cached list of selected DomainItems, used while dragging domains.
        /// </summary>
        private List<DomainItem> cachedSelectedDomainItems;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before drag-selection begins.
        /// </summary>
        private static readonly double DragThreshold = 5;

        /// <summary>
        /// When dragging a connection, this is set to the ConnectorItem that was initially dragged out.
        /// </summary>
        private ConnectorItem draggedOutConnectorItem;

        /// <summary>
        /// The view-model object for the connector that has been dragged out.
        /// </summary>
        private object draggedOutConnectorDataContext;

        /// <summary>
        /// The view-model object for the variable whose connector was dragged out.
        /// </summary>
        private object draggedOutVariableDataContext;

        /// <summary>
        /// The view-model object for the connection that is currently being dragged, or null if none being dragged.
        /// </summary>
        private object draggingConnectionDataContext;

        /// <summary>
        /// A reference to the feedback adorner that is currently in the adorner layer, or null otherwise.
        /// It is used for feedback when dragging a connection over a prospective connector.
        /// </summary>
        private FrameworkElementAdorner feedbackAdorner;

        /// <summary>
        /// Event raised when the user starts to drag a connector.
        /// </summary>
        private void ConnectorItem_DragStarted(object source, ConnectorItemDragStartedEventArgs e)
        {
            this.Focus();

            e.Handled = true;

            this.IsDragging = true;
            this.IsNotDragging = false;
            this.IsDraggingConnection = true;
            this.IsNotDraggingConnection = false;

            this.draggedOutConnectorItem = (ConnectorItem)e.OriginalSource;
            var variableItem = this.draggedOutConnectorItem.ParentVariableItem;
            this.draggedOutVariableDataContext = variableItem.DataContext != null ? variableItem.DataContext : variableItem;
            this.draggedOutConnectorDataContext = this.draggedOutConnectorItem.DataContext != null ? this.draggedOutConnectorItem.DataContext : this.draggedOutConnectorItem;

            //
            // Raise an event so that application code can create a connection and
            // add it to the view-model.
            //
            ConnectionDragStartedEventArgs eventArgs = new ConnectionDragStartedEventArgs(ConnectionDragStartedEvent, this, this.draggedOutVariableDataContext, this.draggedOutConnectorDataContext);
            RaiseEvent(eventArgs);

            //
            // Retrieve the the view-model object for the connection was created by application code.
            //
            this.draggingConnectionDataContext = eventArgs.Connection;

            if (draggingConnectionDataContext == null)
            {
                //
                // Application code didn't create any connection.
                //
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Event raised while the user is dragging a connector.
        /// </summary>
        private void ConnectorItem_Dragging(object source, ConnectorItemDraggingEventArgs e)
        {
            e.Handled = true;

            Trace.Assert((ConnectorItem)e.OriginalSource == this.draggedOutConnectorItem);

            Point mousePoint = Mouse.GetPosition(this);
            //
            // Raise an event so that application code can compute intermediate connection points.
            //
            var connectionDraggingEventArgs =
                new ConnectionDraggingEventArgs(ConnectionDraggingEvent, this,
                        this.draggedOutVariableDataContext, this.draggingConnectionDataContext,
                        this.draggedOutConnectorDataContext);

            RaiseEvent(connectionDraggingEventArgs);

            //
            // Figure out if the connection has been dragged over a connector.
            //

            ConnectorItem connectorDraggedOver = null;
            object connectorDataContextDraggedOver = null;
            bool dragOverSuccess = DetermineConnectorItemDraggedOver(mousePoint, out connectorDraggedOver, out connectorDataContextDraggedOver);
            if (connectorDraggedOver != null)
            {
                //
                // Raise an event so that application code can specify if the connector
                // that was dragged over is valid or not.
                //
                var queryFeedbackEventArgs =
                    new QueryConnectionFeedbackEventArgs(QueryConnectionFeedbackEvent, this, this.draggedOutVariableDataContext, this.draggingConnectionDataContext,
                            this.draggedOutConnectorDataContext, connectorDataContextDraggedOver);

                RaiseEvent(queryFeedbackEventArgs);

                if (queryFeedbackEventArgs.FeedbackIndicator != null)
                {
                    //
                    // A feedback indicator was specified by the event handler.
                    // This is used to indicate whether the connection is good or bad!
                    //
                    AddFeedbackAdorner(connectorDraggedOver, queryFeedbackEventArgs.FeedbackIndicator);
                }
                else
                {
                    //
                    // No feedback indicator specified by the event handler.
                    // Clear any existing feedback indicator.
                    //
                    ClearFeedbackAdorner();
                }
            }
            else
            {
                //
                // Didn't drag over any valid connector.
                // Clear any existing feedback indicator.
                //
                ClearFeedbackAdorner();
            }
        }

        /// <summary>
        /// Event raised when the user has finished dragging a connector.
        /// </summary>
        private void ConnectorItem_DragCompleted(object source, ConnectorItemDragCompletedEventArgs e)
        {
            e.Handled = true;

            Trace.Assert((ConnectorItem)e.OriginalSource == this.draggedOutConnectorItem);

            Point mousePoint = Mouse.GetPosition(this);

            //
            // Figure out if the end of the connection was dropped on a connector.
            //
            ConnectorItem connectorDraggedOver = null;
            object connectorDataContextDraggedOver = null;
            DetermineConnectorItemDraggedOver(mousePoint, out connectorDraggedOver, out connectorDataContextDraggedOver);

            //
            // Now that connection dragging has completed, don't any feedback adorner.
            //
            ClearFeedbackAdorner();

            //
            // Raise an event to inform application code that connection dragging is complete.
            // The application code can determine if the connection between the two connectors
            // is valid and if so it is free to make the appropriate connection in the view-model.
            //
            RaiseEvent(new ConnectionDragCompletedEventArgs(ConnectionDragCompletedEvent, this, this.draggedOutVariableDataContext, this.draggingConnectionDataContext, this.draggedOutConnectorDataContext, connectorDataContextDraggedOver));

            this.IsDragging = false;
            this.IsNotDragging = true;
            this.IsDraggingConnection = false;
            this.IsNotDraggingConnection = true;
            this.draggedOutConnectorDataContext = null;
            this.draggedOutVariableDataContext = null;
            this.draggedOutConnectorItem = null;
            this.draggingConnectionDataContext = null;
        }

        /// <summary>
        /// This function does a hit test to determine which connector, if any, is under 'hitPoint'.
        /// </summary>
        private bool DetermineConnectorItemDraggedOver(Point hitPoint, out ConnectorItem connectorItemDraggedOver, out object connectorDataContextDraggedOver)
        {
            connectorItemDraggedOver = null;
            connectorDataContextDraggedOver = null;

            //
            // Run a hit test 
            //
            HitTestResult result = null;
            VisualTreeHelper.HitTest(variableItemsControl, null,
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

            if (result == null || result.VisualHit == null)
            {
                // Hit test failed.
                return false;
            }

            //
            // Actually want a reference to a 'ConnectorItem'.  
            // The hit test may have hit a UI element that is below 'ConnectorItem' so
            // search up the tree.
            //
            var hitItem = result.VisualHit as FrameworkElement;
            if (hitItem == null)
            {
                return false;
            }
            var connectorItem = WpfUtils.FindVisualParentWithType<ConnectorItem>(hitItem);
            if (connectorItem == null)
            {
                return false;
            }

            var networkView = connectorItem.ParentModelView;
            if (networkView != this)
            {
                //
                // Ensure that dragging over a connector in another NetworkView doesn't
                // return a positive result.
                //
                return false;
            }

            object connectorDataContext = connectorItem;
            if (connectorItem.DataContext != null)
            {
                //
                // If there is a data-context then grab it.
                // When we are using a view-model then it is the view-model
                // object we are interested in.
                //
                connectorDataContext = connectorItem.DataContext;
            }

            connectorItemDraggedOver = connectorItem;
            connectorDataContextDraggedOver = connectorDataContext;

            return true;
        }

        /// <summary>
        /// Add a feedback adorner to a UI element.
        /// This is used to show when a connection can or can't be attached to a particular connector.
        /// 'indicator' will be a view-model object that is transformed into a UI element using a data-template.
        /// </summary>
        private void AddFeedbackAdorner(FrameworkElement adornedElement, object indicator)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);

            if (feedbackAdorner != null)
            {
                if (feedbackAdorner.AdornedElement == adornedElement)
                {
                    // No change.
                    return;
                }

                adornerLayer.Remove(feedbackAdorner);
                feedbackAdorner = null;
            }

            //
            // Create a content control to contain 'indicator'.
            // The view-model object 'indicator' is transformed into a UI element using
            // normal WPF data-template rules.
            //
            ContentControl adornerElement = new ContentControl();
            adornerElement.HorizontalAlignment = HorizontalAlignment.Center;
            adornerElement.VerticalAlignment = VerticalAlignment.Center;
            adornerElement.Content = indicator;

            //
            // Create the adorner and add it to the adorner layer.
            //
            feedbackAdorner = new FrameworkElementAdorner(adornerElement, adornedElement);
            adornerLayer.Add(feedbackAdorner);
        }

        /// <summary>
        /// If there is an existing feedback adorner, remove it.
        /// </summary>
        private void ClearFeedbackAdorner()
        {
            if (feedbackAdorner == null)
            {
                return;
            }

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            adornerLayer.Remove(feedbackAdorner);
            feedbackAdorner = null;
        }

        public ModelView()
        {
            //
            // Create a collection to contain domains.
            //
            this.Variables = new ObservableCollection<object>();
            this.Domains = new ObservableCollection<object>();

            //
            // Create a collection to contain connections.
            //
            this.Connections = new ObservableCollection<object>();

            //
            // Default background is white.
            //
            this.Background = Brushes.White;

            //
            // Add handlers for variable and connector drag events.
            //
            AddHandler(VariableItem.VariableDragStartedEvent, new VariableDragStartedEventHandler(VariableItem_DragStarted));
            AddHandler(VariableItem.VariableDraggingEvent, new VariableDraggingEventHandler(VariableItem_Dragging));
            AddHandler(VariableItem.VariableDragCompletedEvent, new VariableDragCompletedEventHandler(VariableItem_DragCompleted));
            AddHandler(DomainItem.DomainDragStartedEvent, new DomainDragStartedEventHandler(DomainItem_DragStarted));
            AddHandler(DomainItem.DomainDraggingEvent, new DomainDraggingEventHandler(DomainItem_Dragging));
            AddHandler(DomainItem.DomainDragCompletedEvent, new DomainDragCompletedEventHandler(DomainItem_DragCompleted));
            AddHandler(ConnectorItem.ConnectorDragStartedEvent, new ConnectorItemDragStartedEventHandler(ConnectorItem_DragStarted));
            AddHandler(ConnectorItem.ConnectorDraggingEvent, new ConnectorItemDraggingEventHandler(ConnectorItem_Dragging));
            AddHandler(ConnectorItem.ConnectorDragCompletedEvent, new ConnectorItemDragCompletedEventHandler(ConnectorItem_DragCompleted));
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
        /// Event raised when the user starts dragging a connector in the network.
        /// </summary>
        public event ConnectionDragStartedEventHandler ConnectionDragStarted
        {
            add { AddHandler(ConnectionDragStartedEvent, value); }
            remove { RemoveHandler(ConnectionDragStartedEvent, value); }
        }

        /// <summary>
        /// Event raised when the user starts dragging a domain.
        /// </summary>
        public event DomainDragStartedEventHandler DomainDragStarted
        {
            add { AddHandler(DomainDragStartedEvent, value); }
            remove { RemoveHandler(DomainDragStartedEvent, value); }
        }

        /// <summary>
        /// Event raised while user is dragging a domain.
        /// </summary>
        public event DomainDraggingEventHandler DomainDragging
        {
            add { AddHandler(DomainDraggingEvent, value); }
            remove { RemoveHandler(DomainDraggingEvent, value); }
        }

        /// <summary>
        /// Event raised when the user has completed dragging a domain.
        /// </summary>
        public event DomainDragCompletedEventHandler DomainDragCompleted
        {
            add { AddHandler(DomainDragCompletedEvent, value); }
            remove { RemoveHandler(DomainDragCompletedEvent, value); }
        }

        /// <summary>
        /// Event raised while user drags a connection over the connector of a variable in the network.
        /// The event handlers should supply a feedback objects and data-template that displays the 
        /// object as an appropriate graphic.
        /// </summary>
        public event QueryConnectionFeedbackEventHandler QueryConnectionFeedback
        {
            add { AddHandler(QueryConnectionFeedbackEvent, value); }
            remove { RemoveHandler(QueryConnectionFeedbackEvent, value); }
        }

        /// <summary>
        /// Event raised when a connection is being dragged.
        /// </summary>
        public event ConnectionDraggingEventHandler ConnectionDragging
        {
            add { AddHandler(ConnectionDraggingEvent, value); }
            remove { RemoveHandler(ConnectionDraggingEvent, value); }
        }

        /// <summary>
        /// Event raised when the user has completed dragging a connection in the network.
        /// </summary>
        public event ConnectionDragCompletedEventHandler ConnectionDragCompleted
        {
            add { AddHandler(ConnectionDragCompletedEvent, value); }
            remove { RemoveHandler(ConnectionDragCompletedEvent, value); }
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
        /// Collection of domains in the model.
        /// </summary>
        public ObservableCollection<object> Domains
        {
            get
            {
                return (ObservableCollection<object>)GetValue(DomainsProperty);
            }
            private set
            {
                SetValue(DomainsPropertyKey, value);
            }
        }

        /// <summary>
        /// Collection of connections in the network.
        /// </summary>
        public ObservableCollection<object> Connections
        {
            get
            {
                return (ObservableCollection<object>)GetValue(ConnectionsProperty);
            }
            private set
            {
                SetValue(ConnectionsPropertyKey, value);
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
        /// A reference to the collection that is the source used to populate 'domains'.
        /// Used in the same way as 'ItemsSource' in 'ItemsControl'.
        /// </summary>
        public IEnumerable DomainsSource
        {
            get
            {
                return (IEnumerable)GetValue(DomainsSourceProperty);
            }
            set
            {
                SetValue(DomainsSourceProperty, value);
            }
        }

        /// <summary>
        /// A reference to the collection that is the source used to populate 'Connections'.
        /// Used in the same way as 'ItemsSource' in 'ItemsControl'.
        /// </summary>
        public IEnumerable ConnectionsSource
        {
            get
            {
                return (IEnumerable)GetValue(ConnectionsSourceProperty);
            }
            set
            {
                SetValue(ConnectionsSourceProperty, value);
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
        /// Set to 'true' to enable drag out of connectors to create new connections.
        /// </summary>
        public bool EnableConnectionDragging
        {
            get
            {
                return (bool)GetValue(EnableConnectionDraggingProperty);
            }
            set
            {
                SetValue(EnableConnectionDraggingProperty, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'true' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsDraggingConnection
        {
            get
            {
                return (bool)GetValue(IsDraggingConnectionProperty);
            }
            private set
            {
                SetValue(IsDraggingConnectionPropertyKey, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'false' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsNotDraggingConnection
        {
            get
            {
                return (bool)GetValue(IsNotDraggingConnectionProperty);
            }
            private set
            {
                SetValue(IsNotDraggingConnectionPropertyKey, value);
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
        public DataTemplate VariableItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(VariableItemTemplateProperty);
            }
            set
            {
                SetValue(VariableItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets custom style-selection logic for a style that can be applied to each generated container element. 
        /// This is the equivalent to 'ItemTemplateSelector' for ItemsControl.
        /// </summary>
        public DataTemplateSelector VariableItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(VariableItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(VariableItemTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Style that is applied to the item container for each variable item.
        /// This is the equivalent to 'ItemContainerStyle' for ItemsControl.
        /// </summary>
        public Style VariableItemContainerStyle
        {
            get
            {
                return (Style)GetValue(VariableItemContainerStyleProperty);
            }
            set
            {
                SetValue(VariableItemContainerStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display each connection item.
        /// This is the equivalent to 'ItemTemplate' for ItemsControl.
        /// </summary>
        public DataTemplate ConnectionItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ConnectionItemTemplateProperty);
            }
            set
            {
                SetValue(ConnectionItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets custom style-selection logic for a style that can be applied to each generated container element. 
        /// This is the equivalent to 'ItemTemplateSelector' for ItemsControl.
        /// </summary>
        public DataTemplateSelector ConnectionItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ConnectionItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(ConnectionItemTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Style that is applied to the item container for each connection item.
        /// This is the equivalent to 'ItemContainerStyle' for ItemsControl.
        /// </summary>
        public Style ConnectionItemContainerStyle
        {
            get
            {
                return (Style)GetValue(ConnectionItemContainerStyleProperty);
            }
            set
            {
                SetValue(ConnectionItemContainerStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display each variable item.
        /// This is the equivalent to 'ItemTemplate' for ItemsControl.
        /// </summary>
        public DataTemplate DomainItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(DomainItemTemplateProperty);
            }
            set
            {
                SetValue(DomainItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets custom style-selection logic for a style that can be applied to each generated container element. 
        /// This is the equivalent to 'ItemTemplateSelector' for ItemsControl.
        /// </summary>
        public DataTemplateSelector DomainItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(DomainItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(DomainItemTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Style that is applied to the item container for each variable item.
        /// This is the equivalent to 'ItemContainerStyle' for ItemsControl.
        /// </summary>
        public Style DomainItemContainerStyle
        {
            get
            {
                return (Style)GetValue(DomainItemContainerStyleProperty);
            }
            set
            {
                SetValue(DomainItemContainerStyleProperty, value);
            }
        }

        /// <summary>
        /// A reference to currently selected variable.
        /// </summary>
        public object SelectedVariable
        {
            get
            {
                if (variableItemsControl != null)
                {
                    return variableItemsControl.SelectedItem;
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
                if (variableItemsControl != null)
                {
                    variableItemsControl.SelectedItem = value;
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
                if (variableItemsControl != null)
                {
                    return variableItemsControl.SelectedItems;
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
        /// Gets the currently selected domains.
        /// </summary>
        public IList SelectedDomains
        {
            get
            {
                if (variableItemsControl != null)
                {
                    return this.domainItemsControl.SelectedItems;
                }
                else
                {
                    if (initialSelectedDomains == null)
                    {
                        initialSelectedDomains = new List<object>();
                    }

                    return initialSelectedDomains;
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
                VariableItem variableItem = FindAssociatedVariableItem(variable);
                Rect variableRect = new Rect(variableItem.X, variableItem.Y, variableItem.ActualWidth, variableItem.ActualHeight);

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
            if (this.SelectedVariables.Count != this.Variables.Count)
            {
                this.SelectedVariables.Clear();
                foreach (var variable in this.Variables)
                {
                    this.SelectedVariables.Add(variable);
                }
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

        /// <summary>
        /// When connection dragging is progress this function cancels it.
        /// </summary>
        public void CancelConnectionDragging()
        {
            if (!this.IsDraggingConnection)
            {
                return;
            }

            //
            // Now that connection dragging has completed, don't any feedback adorner.
            //
            ClearFeedbackAdorner();

            draggedOutConnectorItem.CancelConnectionDragging();

            this.IsDragging = false;
            this.IsNotDragging = true;
            this.IsDraggingConnection = false;
            this.IsNotDraggingConnection = true;
            this.draggedOutConnectorItem = null;
            this.draggedOutVariableDataContext = null;
            this.draggedOutConnectorDataContext = null;
            this.draggingConnectionDataContext = null;
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

            CancelConnectionDraggingCommand = new RoutedCommand("CancelConnectionDragging", typeof(ModelView));

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

            binding = new CommandBinding();
            binding.Command = CancelConnectionDraggingCommand;
            binding.Executed += new ExecutedRoutedEventHandler(CancelConnectionDragging_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(ModelView), binding);
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
        /// Executes the 'CancelConnectionDragging' command.
        /// </summary>
        private static void CancelConnectionDragging_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ModelView c = (ModelView)sender;
            c.CancelConnectionDragging();
        }

        /// <summary>
        /// Event raised when a new collection has been assigned to the 'VariablesSource' property.
        /// </summary>
        private static void VariablesSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ModelView c = (ModelView)d;

            //
            // Clear 'domains'.
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
        /// Event raised when a new collection has been assigned to the 'VariablesSource' property.
        /// </summary>
        private static void DomainsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ModelView c = (ModelView)d;

            //
            // Clear 'domains'.
            //
            c.Domains.Clear();

            if (e.OldValue != null)
            {
                var notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Unhook events from previous collection.
                    //
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.DomainsSource_CollectionChanged);
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
                        c.Domains.Add(obj);
                    }
                }

                var notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Hook events in new collection.
                    //
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.DomainsSource_CollectionChanged);
                }
            }
        }

        /// <summary>
        /// Event raised when a variable has been added to or removed from the collection assigned to 'VariablesSource'.
        /// </summary>
        private void DomainsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //
                // 'VariablesSource' has been cleared, also clear 'domains'.
                //
                this.Domains.Clear();
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
                        this.Domains.Add(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Event raised when a new collection has been assigned to the 'ConnectionsSource' property.
        /// </summary>
        private static void ConnectionsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ModelView c = (ModelView)d;

            //
            // Clear 'Connections'.
            //
            c.Connections.Clear();

            if (e.OldValue != null)
            {
                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Unhook events from previous collection.
                    //
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.ConnectionsSource_CollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                IEnumerable enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    //
                    // Populate 'Connections' from 'ConnectionsSource'.
                    //
                    foreach (object obj in enumerable)
                    {
                        c.Connections.Add(obj);
                    }
                }

                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Hook events in new collection.
                    //
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.ConnectionsSource_CollectionChanged);
                }
            }
        }

        /// <summary>
        /// Event raised when a connection has been added to or removed from the collection assigned to 'ConnectionsSource'.
        /// </summary>
        private void ConnectionsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //
                // 'ConnectionsSource' has been cleared, also clear 'Connections'.
                //
                Connections.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {
                    //
                    // For each item that has been removed from 'ConnectionsSource' also remove it from 'Connections'.
                    //
                    foreach (object obj in e.OldItems)
                    {
                        Connections.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {
                    //
                    // For each item that has been added to 'ConnectionsSource' also add it to 'Connections'.
                    //
                    foreach (object obj in e.NewItems)
                    {
                        Connections.Add(obj);
                    }
                }
            }
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

            this.variableItemsControl = (VariableItemsControl)this.Template.FindName("PART_VariableItemsControl", this);
            if (this.variableItemsControl == null)
            {
                throw new ApplicationException("Failed to find 'PART_VariableItemsControl' in the visual tree for 'ModelView'.");
            }

            //
            // Synchronize initial selected domains to the VariableItemsControl.
            //
            if (this.initialSelectedVariables != null && this.initialSelectedVariables.Count > 0)
            {
                foreach (var variable in this.initialSelectedVariables)
                {
                    this.variableItemsControl.SelectedItems.Add(variable);
                }
            }

            this.initialSelectedVariables = null; // Don't need this any more.

            this.variableItemsControl.SelectionChanged += new SelectionChangedEventHandler(variableItemsControl_SelectionChanged);

            this.domainItemsControl = (DomainItemsControl)this.Template.FindName("PART_DomainItemsControl", this);
            if (this.domainItemsControl == null)
            {
                throw new ApplicationException("Failed to find 'PART_DomainItemsControl' in the visual tree for 'ModelView'.");
            }

            this.domainItemsControl.SelectionChanged += new SelectionChangedEventHandler(domainItemsControl_SelectionChanged);

            this.connectionItemsControl = (ItemsControl)this.Template.FindName("PART_ConnectionItemsControl", this);
            if (this.connectionItemsControl == null)
            {
                throw new ApplicationException("Failed to find 'PART_ConnectionItemsControl' in the visual tree for 'ModelView'.");
            }

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
        /// Event raised when the selection in 'domainItemsControl' changes.
        /// </summary>
        private void domainItemsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, new SelectionChangedEventArgs(ListBox.SelectionChangedEvent, e.RemovedItems, e.AddedItems));
            }
        }

        /// <summary>
        /// Find the max ZIndex of all the domains.
        /// </summary>
        internal int FindMaxZIndex()
        {
            if (this.variableItemsControl == null)
            {
                return 0;
            }

            int maxZ = 0;

            for (int variableIndex = 0; ; ++variableIndex)
            {
                VariableItem variableItem = (VariableItem)this.variableItemsControl.ItemContainerGenerator.ContainerFromIndex(variableIndex);
                if (variableItem == null)
                {
                    break;
                }

                if (variableItem.ZIndex > maxZ)
                {
                    maxZ = variableItem.ZIndex;
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
        internal VariableItem FindAssociatedVariableItem(object variable)
        {
            VariableItem variableItem = variable as VariableItem;
            if (variableItem == null)
            {
                variableItem = variableItemsControl.FindAssociatedVariableItem(variable);
            }
            return variableItem;
        }

        /// <summary>
        /// Find the DomainItem UI element that is associated with domain.
        /// 'variable' can be a view-model object, in which case the visual-tree
        /// is searched for the associated VariableItem.
        /// Otherwise 'variable' can actually be a 'VariableItem' in which case it is 
        /// simply returned.
        /// </summary>
        internal DomainItem FindAssociatedDomainItem(object variable)
        {
            DomainItem domainItem = variable as DomainItem;
            if (domainItem == null)
            {
                domainItem = domainItemsControl.FindAssociatedDomainItem(variable);
            }
            return domainItem;
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
                this.cachedSelectedVariableItems = new List<VariableItem>();

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


        /// <summary>
        /// Event raised when the user starts to drag a variable.
        /// </summary>
        private void DomainItem_DragStarted(object source, DomainDragStartedEventArgs e)
        {
            e.Handled = true;

            this.IsDragging = true;
            this.IsNotDragging = false;
            this.IsDraggingVariable = true;
            this.IsNotDraggingVariable = false;

            var eventArgs = new DomainDragStartedEventArgs(DomainDragStartedEvent, this, this.SelectedDomains);
            RaiseEvent(eventArgs);

            e.Cancel = eventArgs.Cancel;
        }

        /// <summary>
        /// Event raised while the user is dragging a variable.
        /// </summary>
        private void DomainItem_Dragging(object source, DomainDraggingEventArgs e)
        {
            e.Handled = true;

            //
            // Cache the VariableItem for each selected variable whilst dragging is in progress.
            //
            if (this.cachedSelectedDomainItems == null)
            {
                this.cachedSelectedDomainItems = new List<DomainItem>();

                foreach (var selectedDomain in this.SelectedDomains)
                {
                    var domainItem = FindAssociatedDomainItem(selectedDomain);
                    if (domainItem == null)
                    {
                        throw new ApplicationException("Unexpected code path!");
                    }

                    this.cachedSelectedDomainItems.Add(domainItem);
                }
            }

            // 
            // Update the position of the variable within the Canvas.
            //
            foreach (var domainItem in this.cachedSelectedDomainItems)
            {
                domainItem.X += e.HorizontalChange;
                domainItem.Y += e.VerticalChange;
            }

            var eventArgs = new DomainDraggingEventArgs(DomainDraggingEvent, this, this.SelectedDomains, e.HorizontalChange, e.VerticalChange);
            RaiseEvent(eventArgs);
        }

        /// <summary>
        /// Event raised when the user has finished dragging a domain.
        /// </summary>
        private void DomainItem_DragCompleted(object source, DomainDragCompletedEventArgs e)
        {
            e.Handled = true;

            var eventArgs = new DomainDragCompletedEventArgs(DomainDragCompletedEvent, this, this.SelectedDomains);
            RaiseEvent(eventArgs);

            if (cachedSelectedDomainItems != null)
            {
                cachedSelectedDomainItems = null;
            }

            this.IsDragging = false;
            this.IsNotDragging = true;
            this.IsDraggingVariable = false;
            this.IsNotDraggingVariable = true;
        }
    }
}
