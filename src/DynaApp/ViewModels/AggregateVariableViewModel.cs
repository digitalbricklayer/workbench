using System.Windows;
using Dyna.Core.Models;

namespace DynaApp.ViewModels
{
    public sealed class AggregateVariableViewModel : VariableViewModel
    {
        /// <summary>
        /// Initialize a new aggregate variable with a name and location.
        /// </summary>
        /// <param name="newVariableName">Variable name.</param>
        /// <param name="newVariableLocation">Location.</param>
        public AggregateVariableViewModel(string newVariableName, Point newVariableLocation)
            : base(newVariableName, newVariableLocation)
        {
            this.Model = new AggregateVariableModel(newVariableName, newVariableLocation);
        }

        /// <summary>
        /// Initialize a new aggregate variable with default values.
        /// </summary>
        public AggregateVariableViewModel()
        {
            this.Model = new AggregateVariableModel();
        }

        /// <summary>
        /// Gets or sets the aggregate variable model.
        /// </summary>
        public new AggregateVariableModel Model { get; private set; }
    }
}
