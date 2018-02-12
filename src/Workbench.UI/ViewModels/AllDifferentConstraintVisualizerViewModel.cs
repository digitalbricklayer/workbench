using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class AllDifferentConstraintVisualizerViewModel : ConstraintVisualizerViewModel
    {
        public AllDifferentConstraintVisualizerViewModel(AllDifferentConstraintModel theConstraint, AllDifferentConstraintEditorViewModel theEditor, AllDifferentConstraintViewerViewModel theViewer)
            : base(theConstraint, theEditor, theViewer)
        {
        }
    }

    public class AllDifferentConstraintViewerViewModel : ConstraintViewerViewModel
    {
        public AllDifferentConstraintViewerViewModel(GraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
        }
    }

    public abstract class ConstraintViewerViewModel : ViewerViewModel
    {
        protected ConstraintViewerViewModel(GraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
        }
    }

    public class AllDifferentConstraintEditorViewModel : ConstraintEditorViewModel
    {
        public AllDifferentConstraintEditorViewModel(GraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
        }
    }

    public abstract class ConstraintEditorViewModel : EditorViewModel
    {
        protected ConstraintEditorViewModel(GraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
        }
    }
}
