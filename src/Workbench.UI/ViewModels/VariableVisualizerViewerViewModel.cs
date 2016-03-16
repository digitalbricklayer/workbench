using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Messages;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the variable visualizer in viewer mode.
    /// </summary>
    public sealed class VariableVisualizerViewerViewModel : VisualizerViewerViewModel,
                                                            IHandle<VisualizerBoundMessage>
    {
        private VisualizerModel model;
        private IEventAggregator eventAggregator;

        /// <summary>
        /// Initialize the variable visualizer viewer view model with the variable visualizer model.
        /// </summary>
        /// <param name="theVariableVisualizerModel">Visualizer model.</param>
        /// <param name="theEventAggregator">The event aggregator.</param>
        public VariableVisualizerViewerViewModel(VisualizerModel theVariableVisualizerModel,
                                                 IEventAggregator theEventAggregator)
            : base(theVariableVisualizerModel)
        {
            Contract.Requires<ArgumentNullException>(theVariableVisualizerModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            this.Model = theVariableVisualizerModel;
            this.eventAggregator = theEventAggregator;
            this.eventAggregator.Subscribe(this);
        }

        /// <summary>
        /// Gets or sets the variable model.
        /// </summary>
        public new VisualizerModel Model
        {
            get { return this.model; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                base.Model = value;
                this.model = value;
            }
        }

        /// <summary>
        /// Handles the variable visualizer bound message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(VisualizerBoundMessage message)
        {
            if (message.Variable.Id != this.Model.Binding.VariableId) return;
            this.Binding = message.Variable;
        }
    }
}
