using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class SingletonVariableViewModel : VariableViewModel
    {
        /// <summary>
        /// Initialize the variable view model with the variable model and event aggregator.
        /// </summary>
        /// <param name="theVariableModel">Variable model.</param>
        /// <param name="theEventAggregator">Event aggregator.</param>
        public SingletonVariableViewModel(SingletonVariableGraphicModel theVariableModel, IEventAggregator theEventAggregator)
            : base(theVariableModel, theEventAggregator)
        {
        }
    }
}
