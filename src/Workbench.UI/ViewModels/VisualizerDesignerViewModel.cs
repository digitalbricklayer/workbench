using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public abstract class VisualizerDesignerViewModel : GraphicViewModel
    {
        protected IEventAggregator eventAggregator;
        protected IDataService dataService;
        protected IViewModelService viewModelService;
        private VisualizerModel model;

        protected VisualizerDesignerViewModel(GraphicModel theConstraintModel,
                                            IEventAggregator theEventAggregator,
                                            IDataService theDataService,
                                            IViewModelService theViewModelService)
            : base(theConstraintModel)
        {
            Contract.Requires<ArgumentNullException>(theConstraintModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            this.eventAggregator = theEventAggregator;
            this.dataService = theDataService;
            this.viewModelService = theViewModelService;
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
    }
}
