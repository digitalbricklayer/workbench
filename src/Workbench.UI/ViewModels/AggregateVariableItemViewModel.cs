using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AggregateVariableItemViewModel : VariableItemViewModel
    {
        public AggregateVariableItemViewModel(AggregateVariableModel theAggregateVariableModel)
            : base(theAggregateVariableModel)
        {
            Variables = new BindableCollection<VariableItemViewModel>();
            AggregateVariable = theAggregateVariableModel;
        }

        public AggregateVariableModel AggregateVariable { get; set; }

        /// <summary>
        /// Gets the variables inside the aggregate.
        /// </summary>
        public IObservableCollection<VariableItemViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets or sets the number of variables in the aggregate variable.
        /// </summary>
        public int VariableCount
        {
            get { return AggregateVariable.AggregateCount; }
            set
            {
                AggregateVariable.Resize(value);
                NotifyOfPropertyChange();
            }
        }
    }
}