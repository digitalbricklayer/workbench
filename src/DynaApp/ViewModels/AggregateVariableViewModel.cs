using Dyna.Core.Models;

namespace DynaApp.ViewModels
{
    public sealed class AggregateVariableViewModel : GraphicViewModel
    {
        /// <summary>
        /// Initialize the aggregate variable with default values.
        /// </summary>
        public AggregateVariableViewModel()
        {
            Model = new AggregateVariableModel();
        }

        /// <summary>
        /// Gets or sets the aggregate variable model.
        /// </summary>
        public new AggregateVariableModel Model { get; private set; }
    }
}
