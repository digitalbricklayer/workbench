using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Map the display model to a solution designer view model.
    /// </summary>
    public class DisplayMapper
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDataService dataService;
        private readonly IViewModelService _viewModelService;

        public DisplayMapper(IEventAggregator theEventAggregator,
                             IDataService theDataService,
                             IViewModelService theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            this.eventAggregator = theEventAggregator;
            this.dataService = theDataService;
            this._viewModelService = theViewModelService;
        }

        /// <summary>
        /// Map a display model to a solution designer view model.
        /// </summary>
        /// <param name="theDisplay">Display model.</param>
        /// <returns>Solution designer view model.</returns>
        public SolutionDesignerViewModel MapFrom(DisplayModel theDisplay)
        {
            Contract.Requires<ArgumentNullException>(theDisplay != null);


            var newDesignerViewModel = new SolutionDesignerViewModel(theDisplay);
            foreach (var aVisualizer in theDisplay.Visualizers)
            {
                Debug.Assert(aVisualizer.HasIdentity);

                var newVisualizerViewModel = new VariableVisualizerDesignViewModel(aVisualizer,
                                                                                   this.eventAggregator,
                                                                                   this.dataService,
                                                                                   this._viewModelService);
                newDesignerViewModel.FixupVisualizer(newVisualizerViewModel);
            }

            return newDesignerViewModel;
        }
    }
}
