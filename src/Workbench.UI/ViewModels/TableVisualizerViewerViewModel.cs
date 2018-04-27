using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class TableVisualizerViewerViewModel : ViewerViewModel
    {
        private TableViewModel grid;

        public TableVisualizerViewerViewModel(TableVisualizerModel newVisualizerModel)
            : base(newVisualizerModel)
        {
            Model = newVisualizerModel;
            Grid = new TableViewModel(newVisualizerModel.Table, this);
            GridModel = newVisualizerModel;
        }

        public TableViewModel Grid
        {
            get { return this.grid; }
            set
            {
                this.grid = value;
                NotifyOfPropertyChange();
            }
        }

        public TableVisualizerModel GridModel { get; private set; }

        /// <inheritDoc/>
        public override void Update()
        {
            base.Update();
            // Pick up all changes made to the grid by the grid editor
            Grid = new TableViewModel(GridModel.Table, this);
        }
    }
}
