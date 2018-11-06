using Workbench.Core.Models;
using Workbench.ViewModels.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Dialog based editor for an aggregate variable.
    /// </summary>
    public sealed class AggregateVariableEditorViewModel : DialogViewModel
    {
        private string _variableName;
        private string _domainExpression;
        private int _size;

        /// <summary>
        /// Initialize the aggregate variable edit with default values.
        /// </summary>
        public AggregateVariableEditorViewModel()
        {
            Validator = new AggregateVariableEditorViewModelValidator();
            VariableName = string.Empty;
            DomainExpression = string.Empty;
            Size = AggregateVariableModel.DefaultSize;
        }

        /// <summary>
        /// Gets or sets the variable name.
        /// </summary>
        public string VariableName
        {
            get => _variableName;
            set
            {
                _variableName = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public string DomainExpression
        {
            get => _domainExpression;
            set
            {
                _domainExpression = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
