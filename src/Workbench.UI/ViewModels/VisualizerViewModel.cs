namespace Workbench.ViewModels
{
    public abstract class VisualizerViewModel
    {
        private VisualizerDesignViewModel designer;
        private VisualizerViewerViewModel viewer;

        public VisualizerDesignViewModel Designer
        {
            get { return this.designer; }
            set
            {
                this.designer = (ChessboardVisualizerDesignViewModel)value;
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
