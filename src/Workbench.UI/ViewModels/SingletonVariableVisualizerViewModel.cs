using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class SingletonVariableVisualizerViewModel : VariableVisualizerViewModel
    {
        public SingletonVariableVisualizerViewModel(SingletonVariableModel theVariable, SingletonVariableEditorViewModel theEditor, SingletonVariableViewerViewModel theViewer)
            : base(theVariable, theEditor, theViewer)
        {
            SingletonEditor = theEditor;
            SingletonViewer = theViewer;
        }

        public SingletonVariableEditorViewModel SingletonEditor { get; set; }

        public SingletonVariableViewerViewModel SingletonViewer { get; set; }
    }
}
