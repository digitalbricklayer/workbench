using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class GridVisualizerDesignerViewModel : VisualizerDesignerViewModel
    {
        private GridViewModel _grid;

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
            get { return this._grid; }
            set
            {
                this._grid = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
