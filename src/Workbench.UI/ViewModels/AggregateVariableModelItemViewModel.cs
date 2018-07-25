using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AggregateVariableModelItemViewModel : VariableModelItemViewModel
    {
        public AggregateVariableModelItemViewModel(AggregateVariableModel theAggregateVariableModel)
            : base(theAggregateVariableModel)
        {
            Variables = new BindableCollection<VariableModelItemViewModel>();
            AggregateVariable = theAggregateVariableModel;
        }

        public AggregateVariableModel AggregateVariable { get; set; }

        /// <summary>
        /// Gets the variables inside the aggregate.
        /// </summary>
        public IObservableCollection<VariableModelItemViewModel> Variables { get; private set; }

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