using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class SingletonVariableEditorViewModel : VariableEditorViewModel
    {
        public SingletonVariableEditorViewModel(SingletonVariableGraphicModel theSingletonVariableModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theSingletonVariableModel, theEventAggregator, theDataService, theViewModelService)
        {
            Model = theSingletonVariableModel;
            SingletonVariableGraphic = theSingletonVariableModel;
        }

        public SingletonVariableGraphicModel SingletonVariableGraphic { get; set; }

        public override bool IsAggregate => false;
    }
}