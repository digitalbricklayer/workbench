using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AggregateVariableVisualizerViewModel  : VariableVisualizerViewModel
    {
        public AggregateVariableVisualizerViewModel(AggregateVariableModel theVariable, AggregateVariableEditorViewModel theEditor, AggregateVariableViewerViewModel theViewer)
            : base(theVariable, theEditor, theViewer)
        {
            AggregateEditor = theEditor;
            AggregateViewer = theViewer;
        }

        public AggregateVariableViewerViewModel AggregateViewer { get; set; }

        public AggregateVariableEditorViewModel AggregateEditor { get; set; }
    }
}
