using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class SingletonVariableEditorViewModel : VariableEditorViewModel
    {
        public SingletonVariableEditorViewModel(SingletonVariableGraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
            Model = theGraphicModel;
            SingletonVariableGraphic = theGraphicModel;
        }

        public SingletonVariableGraphicModel SingletonVariableGraphic { get; set; }

        public override bool IsAggregate => false;
    }
}