using Caliburn.Micro;

namespace Workbench.ViewModels
{
    public abstract class VisualizerViewModel : Screen
    {
        private EditorViewModel designer;
        private VisualizerViewerViewModel viewer;

        /// <summary>
        /// Gets or sets the visualizer designer.
        /// </summary>
        public EditorViewModel Designer
        {
            get { return this.designer; }
            set
            {
                this.designer = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the visualizer viewer.
        /// </summary>
        public VisualizerViewerViewModel Viewer
        {
            get { return this.viewer; }
            set
            {
                this.viewer = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
