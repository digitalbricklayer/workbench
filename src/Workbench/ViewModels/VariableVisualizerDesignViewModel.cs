﻿using System;
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
                                                            IHandle<AggregateVariableAddedMessage>
    {
        private VariableViewModel boundTo;
        private VariableVisualizerModel model;
        private IObservableCollection<string> availableVariables;
        private string selectedVariable;
        private readonly IEventAggregator eventAggregator;
        private readonly IDataService dataService;

        /// <summary>
        /// Initialize the variable visualizer design view model with the 
        /// variable visualizer model.
        /// </summary>
        /// <param name="theVariableVisualizerModel">Visualizer model.</param>
        /// <param name="theEventAggregator">The event aggregator.</param>
        /// <param name="theDataService">Data service.</param>
        public VariableVisualizerDesignViewModel(VariableVisualizerModel theVariableVisualizerModel,
                                                 IEventAggregator theEventAggregator,
                                                 IDataService theDataService)
            : base(theVariableVisualizerModel)
        {
            if (theVariableVisualizerModel == null)
                throw new ArgumentNullException("theVariableVisualizerModel");

            if (theEventAggregator == null)
                throw new ArgumentNullException("theEventAggregator");

            this.AvailableVariables = new BindableCollection<string>();
            this.Model = theVariableVisualizerModel;
            this.eventAggregator = theEventAggregator;
            this.dataService = theDataService;
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
        /// Gets or sets the variable the visualizer is bound to.
        /// </summary>
        public VariableViewModel BoundTo
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
        /// Gets or sets the available variables available to bind to.
        /// </summary>
        public IObservableCollection<string> AvailableVariables
        {
            get { return availableVariables; }
            set
            {
                availableVariables = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the selected variable to bind to.
        /// </summary>
        public string SelectedVariable
        {
            get { return selectedVariable; }
            set
            {
                selectedVariable = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Handle the singleton variable added message.
        /// </summary>
        /// <param name="theMessage">Variable added message.</param>
        public void Handle(SingletonVariableAddedMessage theMessage)
        {
            this.AvailableVariables.Add(theMessage.NewVariableName);
        }

        /// <summary>
        /// Handle the aggregate variable added message.
        /// </summary>
        /// <param name="message">Variable added message.</param>
        public void Handle(AggregateVariableAddedMessage message)
        {
            this.AvailableVariables.Add(message.NewVariableName);
        }

        /// <summary>
        /// Called when activating.
        /// </summary>
        protected override void OnActivate()
        {
            this.PopulateAvailableVariables();
            base.OnActivate();
            this.eventAggregator.Subscribe(this);
        }

        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name="close">Indicates whether this instance will be closed.</param>
        protected override void OnDeactivate(bool close)
        {
            this.eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        /// <summary>
        /// Populate the available variables collection.
        /// </summary>
        private void PopulateAvailableVariables()
        {
            var theWorkspace = this.dataService.GetWorkspace();
            var theModel = theWorkspace.Model;
            foreach (var aVariable in theModel.Variables)
            {
                this.AvailableVariables.Add(aVariable.Name);
            }
        }
    }
}