using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AggregateVariableViewerViewModel : VariableViewerViewModel
    {
        public AggregateVariableViewerViewModel(AggregateVariableGraphicModel theAggregateVariableGraphic)
            : base(theAggregateVariableGraphic)
        {
            AggregateVariableGraphic = theAggregateVariableGraphic;
        }

        public AggregateVariableGraphicModel AggregateVariableGraphic { get; set; }

        /// <summary>
        /// Gets the variables inside the aggregate.
        /// </summary>
        public IObservableCollection<VariableEditorViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets or sets the number of variables in the aggregate variable.
        /// </summary>
        public int VariableCount
        {
            get
            {
                return AggregateVariableGraphic.AggregateCount;
            }
        }

        /// <summary>
        /// Gets whether the variable is an aggregate.
        /// </summary>
        public override bool IsAggregate => true;

    }
}