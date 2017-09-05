using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class GridVisualizerViewerViewModel : VisualizerViewerViewModel
    {
        private GridViewModel grid;

        public GridVisualizerViewerViewModel(GridVisualizerModel newVisualizerModel)
            : base(newVisualizerModel)
        {
            Model = newVisualizerModel;
            Grid = new GridViewModel(newVisualizerModel.Grid);
            GridModel = newVisualizerModel;
        }

        public GridViewModel Grid
        {
            get { return this.grid; }
            set
            {
                this.grid = value;
                NotifyOfPropertyChange();
            }
        }

        public GridVisualizerModel GridModel { get; private set; }

        /// <inheritDoc/>
        public override void Update()
        {
            base.Update();
            // Pick up all changes made to the grid by the grid editor
            Grid = new GridViewModel(GridModel.Grid);
        }
    }
}
