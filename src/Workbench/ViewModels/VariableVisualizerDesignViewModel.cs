using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the variable visualizer in design mode.
    /// </summary>
    public sealed class VariableVisualizerDesignViewModel : GraphicViewModel,
                                                            IHandle<SingletonVariableAddedMessage>,
                                                            IHandle<AggregateVariableAddedMessage>,
                                                            IHandle<VariableDeletedMessage>
    {
        private VariableVisualizerModel model;
        private IObservableCollection<VariableViewModel> availableVariables;
        private VariableViewModel selectedVariable;
        private readonly IEventAggregator eventAggregator;
        private readonly IDataService dataService;
        private readonly IViewModelService _viewModelService;

        /// <summary>
        /// Initialize the variable visualizer design view model with the 
        /// variable visualizer model.
        /// </summary>
        /// <param name="theVariableVisualizerModel">Visualizer model.</param>
        /// <param name="theEventAggregator">The event aggregator.</param>
        /// <param name="theDataService">Data service.</param>
        /// <param name="theViewModelService">The workspace.</param>
        public VariableVisualizerDesignViewModel(VariableVisualizerModel theVariableVisualizerModel,
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
            this._viewModelService = theViewModelService;
            if (this.Model.Binding != null && !string.IsNullOrEmpty(this.Model.Binding.Name))
                SelectVariableBinding();
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
                Contract.Requires<ArgumentNullException>(value != null);

                base.Model = value;
                this.model = value;
            }
        }

        /// <summary>
        /// Gets or sets the available variables available to bind to.
        /// </summary>
        public IObservableCollection<VariableViewModel> AvailableVariables
        {
            get { return availableVariables; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);

                availableVariables = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the selected variable to bind to.
        /// </summary>
        public VariableViewModel SelectedVariable
        {
            get { return this.selectedVariable; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);

                this.selectedVariable = value;
                if (this.SelectedVariable != null)
                {
                    var variableToBindTo = this.dataService.GetVariableByName(this.SelectedVariable.Name);
                    this.Model.BindTo(variableToBindTo);
                }
                NotifyOfPropertyChange();

                var newVariableBoundMessage 
                    = new VariableVisualizerBoundMessage(this.Model,
                                                         this.SelectedVariable);
                this.eventAggregator.BeginPublishOnUIThread(newVariableBoundMessage);
            }
        }

        /// <summary>
        /// Handle the singleton variable added message.
        /// </summary>
        /// <param name="theMessage">Variable added message.</param>
        public void Handle(SingletonVariableAddedMessage theMessage)
        {
            this.AvailableVariables.Add(theMessage.NewVariable);
        }

        /// <summary>
        /// Handle the aggregate variable added message.
        /// </summary>
        /// <param name="message">Variable added message.</param>
        public void Handle(AggregateVariableAddedMessage message)
        {
            this.AvailableVariables.Add(message.Added);
        }

        /// <summary>
        /// Handle the variable delete message.
        /// </summary>
        /// <param name="message">Variable deleted message.</param>
        public void Handle(VariableDeletedMessage message)
        {
            this.AvailableVariables.Remove(message.Deleted);
            if (this.SelectedVariable == message.Deleted)
                this.SelectedVariable = null;
        }

        /// <summary>
        /// Called when initializing the visualizer.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.PopulateAvailableVariables();
        }

        /// <summary>
        /// Populate the available variables collection.
        /// </summary>
        private void PopulateAvailableVariables()
        {
            this.AvailableVariables.Clear();
            var allVariables = this._viewModelService.GetAllVariables();
            foreach (var aVariable in allVariables)
            {
                this.AvailableVariables.Add(aVariable);
            }
        }

        private void SelectVariableBinding()
        {
            this.selectedVariable = this._viewModelService.GetVariableByIdentity(this.Model.Binding.VariableId);
        }
    }
}
