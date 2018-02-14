using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AllDifferentConstraintVisualizerViewModel : ConstraintVisualizerViewModel
    {
        public AllDifferentConstraintVisualizerViewModel(AllDifferentConstraintModel theConstraint, AllDifferentConstraintEditorViewModel theEditor, AllDifferentConstraintViewerViewModel theViewer)
            : base(theConstraint, theEditor, theViewer)
        {
            AllDifferentEditor = theEditor;
        }

        public override bool IsValid { get; }
        public AllDifferentConstraintEditorViewModel AllDifferentEditor { get; }
    }
}
