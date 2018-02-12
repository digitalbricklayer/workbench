using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ConstraintVisualizerViewModel : VisualizerViewModel
    {
        protected ConstraintVisualizerViewModel(ConstraintModel theConstraint, ConstraintEditorViewModel theEditor, ConstraintViewerViewModel theViewer)
            : base(theConstraint, theEditor, theViewer)
        {
        }
    }
}
