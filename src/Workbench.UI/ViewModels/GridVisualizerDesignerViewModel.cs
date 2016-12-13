using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class GridVisualizerDesignerViewModel : VisualizerDesignerViewModel
    {
        private GridViewModel grid;

        public GridVisualizerDesignerViewModel(GridVisualizerModel theMapModel,
                                              IEventAggregator theEventAggregator,
                                              IDataService theDataService,
                                              IViewModelService theViewModelService)
            : base(theMapModel, theEventAggregator, theDataService, theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theMapModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            Model = theMapModel;
            Grid = new GridViewModel(theMapModel.Grid);
        }

        /// <summary>
        /// Gets or sets the map view model.
        /// </summary>
        public GridViewModel Grid
        {
            get { return this.grid; }
            set
            {
                this.grid = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddColumn(GridColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Grid.AddColumn(newColumn);
        }

        public void Resize(int columns, int rows)
        {
            Grid.Resize(columns, rows);
        }
    }
}
