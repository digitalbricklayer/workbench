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
    public sealed class VariableVisualizerViewerViewModel : GraphicViewModel,
                                                            IHandle<VariableVisualizerBoundMessage>
    {
        private VariableViewModel boundTo;
        private VariableVisualizerModel model;
        private ValueViewModel value;
        private IEventAggregator eventAggregator;

        /// <summary>
        /// Initialize the variable visualizer viewer view model with the variable visualizer model.
        /// </summary>
        /// <param name="theVariableVisualizerModel">Visualizer model.</param>
        /// <param name="theEventAggregator">The event aggregator.</param>
        public VariableVisualizerViewerViewModel(VariableVisualizerModel theVariableVisualizerModel,
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
        public new VariableVisualizerModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = value;
            }
        }

        /// <summary>
        /// Gets or sets the variable binding.
        /// </summary>
        public VariableViewModel Binding
        {
            get
            {
                return this.boundTo;
            }
            set
            {
                this.boundTo = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the variable value;
        /// </summary>
        public ValueViewModel Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Unbind the value from viewer.
        /// </summary>
        public void Unbind()
        {
            this.Value = null;
        }

        /// <summary>
        /// Handles the variable visualizer bound message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(VariableVisualizerBoundMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            this.Binding = message.Variable;
        }
    }
}
