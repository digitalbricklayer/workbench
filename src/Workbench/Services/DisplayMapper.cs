using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Map the display model to a solution designer view model.
    /// </summary>
    internal class DisplayMapper
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDataService dataService;

        public DisplayMapper(IEventAggregator theEventAggregator,
                             IDataService theDataService)
        {
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);

            this.eventAggregator = theEventAggregator;
            this.dataService = theDataService;
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
                var newVisualizerViewModel = new VariableVisualizerDesignViewModel(aVisualizer,
                                                                                   this.eventAggregator,
                                                                                   this.dataService);
                newDesignerViewModel.AddVisualizer(newVisualizerViewModel);
            }

            return newDesignerViewModel;
        }
    }
}