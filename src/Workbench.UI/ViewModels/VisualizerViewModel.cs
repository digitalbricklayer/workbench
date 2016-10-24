namespace Workbench.ViewModels
{
    public abstract class VisualizerViewModel
    {
        private VisualizerDesignerViewModel designer;
        private VisualizerViewerViewModel viewer;

        public VisualizerDesignerViewModel Designer
        {
            get { return this.designer; }
            set
            {
                this.designer = (ChessboardVisualizerDesignerViewModel)value;
            }
        }

        public VisualizerViewerViewModel Viewer
        {
            get { return this.viewer; }
            set
            {
                this.viewer = value;
            }
        }
    }
}
