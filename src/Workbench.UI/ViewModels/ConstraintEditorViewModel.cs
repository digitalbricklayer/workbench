using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public abstract class ConstraintEditorViewModel : EditorViewModel
    {
        protected ConstraintEditorViewModel(ConstraintGraphicModel theConstraintGraphic, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theConstraintGraphic, theEventAggregator, theDataService, theViewModelService)
        {
            ConstraintGraphic = theConstraintGraphic;
        }

        public ConstraintGraphicModel ConstraintGraphic { get; }
    }
}