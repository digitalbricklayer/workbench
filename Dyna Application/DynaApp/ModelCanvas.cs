using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DynaApp.Tools;

namespace DynaApp
{
    /// <summary>
    /// Canvas onto which the model is drawn.
    /// </summary>
    sealed class ModelCanvas : Canvas
    {
        private VisualCollection itemsList;
        private Tool[] tools = 
        {
            new DomainTool(),
            new VariableTool()
        };

        public ModelCanvas()
        {
            this.itemsList = new VisualCollection(this);

            this.Loaded += new RoutedEventHandler(WorkspaceCanvas_Loaded);
            this.MouseDown += new MouseButtonEventHandler(WorkspaceCanvas_MouseDown);
            this.MouseMove += new MouseEventHandler(WorkspaceCanvas_MouseMove);
            this.MouseUp += new MouseButtonEventHandler(WorkspaceCanvas_MouseUp);
            this.KeyDown += new KeyEventHandler(WorkspaceCanvas_KeyDown);
            this.LostMouseCapture += new MouseEventHandler(WorkspaceCanvas_LostMouseCapture);
        }

        /// <summary>
        /// Mouse down.
        /// Left button down event is passed to active tool.
        /// Right button down event is handled in this class.
        /// </summary>
        void WorkspaceCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
#if false
            if (tools[(int)Tool] == null)
            {
                return;
            }


            this.Focus();


            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    HandleDoubleClick(e);        // special case for GraphicsText
                }
                else
                {
                    tools[(int)Tool].OnMouseDown(this, e);
                }

                UpdateState();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ShowContextMenu(e);
            }
#endif
        }

        /// <summary>
        /// Mouse move.
        /// Moving without button pressed or with left button pressed
        /// is passed to active tool.
        /// </summary>
        void WorkspaceCanvas_MouseMove(object sender, MouseEventArgs e)
        {
#if false
            if (tools[(int)Tool] == null)
            {
                return;
            }

            if (e.MiddleButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                tools[(int)Tool].OnMouseMove(this, e);

                UpdateState();
            }
            else
            {
                this.Cursor = HelperFunctions.DefaultCursor;
            }
#endif
        }

        /// <summary>
        /// Mouse up event.
        /// Left button up event is passed to active tool.
        /// </summary>
        void WorkspaceCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
#if false
            if (tools[(int)Tool] == null)
            {
                return;
            }


            if (e.ChangedButton == MouseButton.Left)
            {
                tools[(int)Tool].OnMouseUp(this, e);

                UpdateState();
            }
#endif
        }


        /// <summary>
        /// Initialization after control is loaded
        /// </summary>
        void WorkspaceCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focusable = true;      // to handle keyboard messages
        }

        /// <summary>
        /// Mouse capture is lost
        /// </summary>
        void WorkspaceCanvas_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
#if false
                CancelCurrentOperation();
                UpdateState();
#endif
            }
        }

        /// <summary>
        /// Handle keyboard input
        /// </summary>
        void WorkspaceCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            // Esc key stops currently active operation
            if (e.Key == Key.Escape)
            {
                if (this.IsMouseCaptured)
                {
#if false
                    CancelCurrentOperation();
                    UpdateState();
#endif
                }
            }
        }

    }
}
