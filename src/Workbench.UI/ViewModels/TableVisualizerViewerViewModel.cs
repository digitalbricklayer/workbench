using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class TableVisualizerViewerViewModel : ViewerViewModel
    {
        private TableViewModel grid;
        private readonly IEventAggregator eventAggregator;

        public TableVisualizerViewerViewModel(TableVisualizerModel newVisualizerModel, IEventAggregator theEventAggregator)
            : base(newVisualizerModel)
        {
            Contract.Requires<ArgumentNullException>(newVisualizerModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            Model = newVisualizerModel;
            this.eventAggregator = theEventAggregator;
            this.eventAggregator.Subscribe(this);
            Grid = new TableViewModel(newVisualizerModel.Table, theEventAggregator);
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
            Grid = new TableViewModel(GridModel.Table, this.eventAggregator);
        }
    }
}
