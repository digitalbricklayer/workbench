using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class MapVisualizerDesignerViewModel : VisualizerDesignerViewModel
    {
        private MapViewModel map;

        public MapVisualizerDesignerViewModel(MapVisualizerModel theMapModel,
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
            Map = new MapViewModel(theMapModel.Model);
        }

        /// <summary>
        /// Gets or sets the map view model.
        /// </summary>
        public MapViewModel Map
        {
            get { return this.map; }
            set
            {
                this.map = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
