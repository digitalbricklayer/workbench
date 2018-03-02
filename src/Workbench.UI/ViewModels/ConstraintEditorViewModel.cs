using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public abstract class ConstraintEditorViewModel : EditorViewModel
    {
        private ConstraintGraphicModel constraintGraphic;

        protected ConstraintEditorViewModel(ConstraintGraphicModel theConstraintGraphic, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theConstraintGraphic, theEventAggregator, theDataService, theViewModelService)
        {
            ConstraintGraphic = theConstraintGraphic;
        }

        /// <summary>
        /// Gets whether the expression is a valid expression.
        /// </summary>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Gets or sets the constraint constraint graphic.
        /// </summary>
        public ConstraintGraphicModel ConstraintGraphic
        {
            get { return this.constraintGraphic; }
            set
            {
                this.constraintGraphic = value;
                NotifyOfPropertyChange();
            }
        }
    }
}