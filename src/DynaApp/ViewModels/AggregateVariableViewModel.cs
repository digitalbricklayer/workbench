using System.Collections.ObjectModel;
using System.Linq;
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
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Variables.Add(new VariableViewModel(newVariableName + "1", new VariableDomainExpressionViewModel()));
            this.Model = new AggregateVariableModel(newVariableName, newVariableLocation);
        }

        /// <summary>
        /// Initialize a new aggregate variable with a name and location.
        /// </summary>
        /// <param name="newVariableName">Variable name.</param>
        /// <param name="size">Size of the aggregate.</param>
        /// <param name="domainExpression">Domain expression.</param>
        public AggregateVariableViewModel(string newVariableName, int size, VariableDomainExpressionViewModel domainExpression)
            : base(newVariableName)
        {
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Variables.Add(new VariableViewModel(newVariableName+"1", new VariableDomainExpressionViewModel()));
            this.Model = new AggregateVariableModel(newVariableName, size, domainExpression.Model);
        }

        /// <summary>
        /// Initialize a new aggregate variable with default values.
        /// </summary>
        public AggregateVariableViewModel()
        {
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Variables.Add(new VariableViewModel("x" + "1", new VariableDomainExpressionViewModel()));
            this.Model = new AggregateVariableModel();
        }

        /// <summary>
        /// Gets or sets the aggregate variable model.
        /// </summary>
        public new AggregateVariableModel Model { get; private set; }

        /// <summary>
        /// Gets the variables inside the aggregate.
        /// </summary>
        public ObservableCollection<VariableViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets the variable count range expression.
        /// </summary>
        public string Range
        {
            get
            {
                if (this.Variables.Any())
                    return "1.."+this.Variables.Count;
                return "0";
            }
        }
    }
}
