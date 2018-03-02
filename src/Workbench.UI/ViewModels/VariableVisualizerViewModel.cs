using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class VariableVisualizerViewModel : VisualizerViewModel
    {
        public VariableVisualizerViewModel(VariableModel theVariable, VariableEditorViewModel theEditor, VariableViewerViewModel theViewer)
            : base(theVariable, theEditor, theViewer)
        {
            Variable = theVariable;
            VariableEditor = theEditor;
            VariableViewer = theViewer;
        }

        public VariableViewerViewModel VariableViewer { get; set; }

        public VariableEditorViewModel VariableEditor { get; set; }

        public VariableModel Variable { get; }
    }
}