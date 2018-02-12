using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class ExpressionConstraintVisualizerViewModel : ConstraintVisualizerViewModel
    {
        public ExpressionConstraintVisualizerViewModel(ExpressionConstraintModel theConstraint, ExpressionConstraintEditorViewModel theEditor, ExpressionConstraintViewerViewModel theViewer)
            : base(theConstraint, theEditor, theViewer)
        {
        }
    }

    public class ExpressionConstraintViewerViewModel : ConstraintViewerViewModel
    {
        public ExpressionConstraintViewerViewModel(GraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
        }
    }

    public class ExpressionConstraintEditorViewModel : ConstraintEditorViewModel
    {
        public ExpressionConstraintEditorViewModel(GraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
        }
    }
}
