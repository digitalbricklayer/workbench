using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AggregateVariableEditorViewModel : ValidatingScreen
    {
        private string _variableName;
        private string _domainExpression;
        private int _size;

        /// <summary>
        /// Initialize the aggregate variable edit with default values.
        /// </summary>
        public AggregateVariableEditorViewModel()
        {
            AddValidationRule(() => VariableName).Condition(() => string.IsNullOrWhiteSpace(VariableName)).Message("Variable name must not be blank");
            AddValidationRule(() => DomainExpression).Condition(() => string.IsNullOrWhiteSpace(DomainExpression)).Message("Domain expression must not be empty");
            AddValidationRule(() => Size).Condition(() => Size <= 0).Message("Size must be at least one");
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
                NotifyOfPropertyChange(nameof(CanAccept));
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
                NotifyOfPropertyChange(nameof(CanAccept));
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
                NotifyOfPropertyChange(nameof(CanAccept));
            }
        }

        /// <summary>
        /// Can Accept be executed.
        /// </summary>
        public bool CanAccept => !HasError;

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void Accept()
        {
            TryClose(true);
        }
    }
}
