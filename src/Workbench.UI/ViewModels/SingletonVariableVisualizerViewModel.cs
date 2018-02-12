using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

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

    public class SingletonVariableEditorViewModel : VariableEditorViewModel
    {
        public SingletonVariableEditorViewModel(SingletonVariableGraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
            Model = theGraphicModel;
            SingletonVariableGraphic = theGraphicModel;
        }

        public SingletonVariableGraphicModel SingletonVariableGraphic { get; set; }
    }

    public class SingletonVariableViewerViewModel : VariableViewerViewModel
    {
        public SingletonVariableViewerViewModel(SingletonVariableGraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
            Model = theGraphicModel;
        }
    }
}
