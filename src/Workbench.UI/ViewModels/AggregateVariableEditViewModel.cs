using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AggregateVariableEditViewModel : Screen
    {
        private string _variableName;
        private string _domainExpression;
        private int _size;

        /// <summary>
        /// Initialize the aggregate variable edit with default values.
        /// </summary>
        public AggregateVariableEditViewModel()
        {
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

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void AcceptButton()
        {
            TryClose(true);
        }
    }
}
