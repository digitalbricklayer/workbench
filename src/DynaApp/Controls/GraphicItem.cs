using System.Windows;
using System.Windows.Controls;
using DynaApp.Views;

namespace DynaApp.Controls
{
    public abstract class GraphicItem : ListBoxItem
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(GraphicItem),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(GraphicItem),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ZIndexProperty =
            DependencyProperty.Register("ZIndex", typeof(int), typeof(GraphicItem),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        internal static readonly DependencyProperty ParentModelViewProperty =
            DependencyProperty.Register("ParentModelView", typeof(ModelView), typeof(GraphicItem),
                new FrameworkPropertyMetadata(ParentModelView_PropertyChanged));

        #endregion Dependency Property/Event Definitions

        /// <summary>
        /// The point the mouse was last at when dragging.
        /// </summary>
        protected Point lastMousePoint;

        /// <summary>
        /// Set to 'true' when left mouse button is held down.
        /// </summary>
        protected bool isLeftMouseDown;

        /// <summary>
        /// Set to 'true' when left mouse button and the control key are held down.
        /// </summary>
        protected bool isLeftMouseAndControlDown;

        /// <summary>
        /// Set to 'true' when dragging has started.
        /// </summary>
        protected bool isDragging;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before dragging begins.
        /// </summary>
        protected const double DragThreshold = 5;

        protected GraphicItem()
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

        internal abstract void LeftMouseUpSelectionLogic();
        internal abstract void LeftMouseDownSelectionLogic();

        internal abstract void RightMouseDownSelectionLogic();
        
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
        /// Bring the variable to the front of other elements.
        /// </summary>
        internal void BringToFront()
        {
            if (this.ParentModelView == null) return;

            int maxZ = this.ParentModelView.FindMaxZIndex();
            this.ZIndex = maxZ + 1;
        }

        /// <summary>
        /// Event raised when the ParentModelView property has changed.
        /// </summary>
        private static void ParentModelView_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            //
            // Bring new domains to the front of the z-order.
            //
            var graphicItem = (GraphicItem)o;
            graphicItem.BringToFront();
        }
    }
}
