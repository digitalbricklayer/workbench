using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class GridVisualizerViewerViewModel : VisualizerViewerViewModel
    {
        private GridViewModel _grid;

        public GridVisualizerViewerViewModel(GridVisualizerModel newVisualizerModel)
            : base(newVisualizerModel)
        {
            Model = newVisualizerModel;
            Grid = new GridViewModel(newVisualizerModel.Grid);
            MapModel = newVisualizerModel;
        }

        public GridViewModel Grid
        {
            get { return this._grid; }
            set
            {
                this._grid = value;
                NotifyOfPropertyChange();
            }
        }

        public GridVisualizerModel MapModel { get; private set; }
    }
}
