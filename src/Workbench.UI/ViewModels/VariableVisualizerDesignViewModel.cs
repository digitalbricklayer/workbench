using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the variable visualizer in design mode.
    /// </summary>
    public sealed class VariableVisualizerDesignViewModel : VisualizerDesignViewModel
    {
        /// <summary>
        /// Initialize the variable visualizer design view model with the 
        /// variable visualizer model.
        /// </summary>
        /// <param name="theVariableVisualizerModel">Visualizer model.</param>
        /// <param name="theEventAggregator">The event aggregator.</param>
        /// <param name="theDataService">Data service.</param>
        /// <param name="theViewModelService">The workspace.</param>
        public VariableVisualizerDesignViewModel(VisualizerModel theVariableVisualizerModel,
                                                 IEventAggregator theEventAggregator,
                                                 IDataService theDataService,
                                                 IViewModelService theViewModelService)
            : base(theVariableVisualizerModel)
        {
            Contract.Requires<ArgumentNullException>(theVariableVisualizerModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            this.AvailableVariables = new BindableCollection<VariableViewModel>();
            this.Model = theVariableVisualizerModel;
            this.eventAggregator = theEventAggregator;
            this.dataService = theDataService;
            this.viewModelService = theViewModelService;
            if (this.Model.Binding != null && !string.IsNullOrEmpty(this.Model.Binding.Name))
                SelectVariableBinding();
            this.eventAggregator.Subscribe(this);
        }
    }
}
