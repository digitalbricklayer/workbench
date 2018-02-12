using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class AggregateVariableEditorViewModel : VariableEditorViewModel
    {
        public AggregateVariableEditorViewModel(AggregateVariableGraphicModel theGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
            AggregateVariableGraphic = theGraphicModel;
        }

        public AggregateVariableGraphicModel AggregateVariableGraphic { get; set; }
        public string NumberVariables { get; set; }
    }
}