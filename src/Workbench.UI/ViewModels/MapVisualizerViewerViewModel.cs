using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class MapVisualizerViewerViewModel : VisualizerViewerViewModel
    {
        private MapViewModel map;

        public MapVisualizerViewerViewModel(MapVisualizerModel newVisualizerModel)
            : base(newVisualizerModel)
        {
            Model = newVisualizerModel;
            Map = new MapViewModel(newVisualizerModel.Model);
            MapModel = newVisualizerModel;
        }

        public MapViewModel Map
        {
            get { return this.map; }
            set
            {
                this.map = value;
                NotifyOfPropertyChange();
            }
        }

        public MapVisualizerModel MapModel { get; private set; }
    }
}
