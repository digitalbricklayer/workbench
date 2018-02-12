using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AggregateVariableViewerViewModel : VariableViewerViewModel
    {
        public AggregateVariableViewerViewModel(AggregateVariableGraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
            AggregateVariableGraphic = theGraphicModel;
        }

        public AggregateVariableGraphicModel AggregateVariableGraphic { get; set; }
    }
}