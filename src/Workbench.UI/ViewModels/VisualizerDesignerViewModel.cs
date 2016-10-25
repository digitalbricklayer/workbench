using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public abstract class VisualizerDesignerViewModel : GraphicViewModel
    {
        /// <summary>
        /// Is the name currently being edited.
        /// </summary>
        private bool isTitleEditing;

        protected IEventAggregator eventAggregator;
        protected IDataService dataService;
        protected IViewModelService viewModelService;
        private VisualizerModel model;

        protected VisualizerDesignerViewModel(GraphicModel theGraphicModel,
                                            IEventAggregator theEventAggregator,
                                            IDataService theDataService,
                                            IViewModelService theViewModelService)
            : base(theGraphicModel)
        {
            Contract.Requires<ArgumentNullException>(theGraphicModel != null);
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

        /// <summary>
        /// Gets or sets the graphic name.
        /// </summary>
        public virtual string Title
        {
            get { return Model.Title; }
            set
            {
                Model.Title = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets whether the title is being edited.
        /// </summary>
        public bool IsTitleEditing
        {
            get { return this.isTitleEditing; }
            set
            {
                this.isTitleEditing = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the edit title command.
        /// </summary>
        public ICommand EditTitleCommand
        {
            get
            {
                return new CommandHandler(() => IsTitleEditing = true);
            }
        }
    }
}
