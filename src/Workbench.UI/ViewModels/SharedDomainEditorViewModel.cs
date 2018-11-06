using Workbench.Validators;

namespace Workbench.ViewModels
{
    public class SharedDomainEditorViewModel : DialogViewModel
    {
        private string _domainName;
        private string _domainExpression;

        /// <summary>
        /// Initialize the shared domain editor with default values.
        /// </summary>
        public SharedDomainEditorViewModel()
        {
            Validator = new SharedDomainEditorViewModelValidator();
            DomainName = string.Empty;
            DomainExpression = string.Empty;
        }

        /// <summary>
        /// Gets or sets the domain name.
        /// </summary>
        public string DomainName
        {
            get => _domainName;
            set
            {
                _domainName = value;
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
    }
}
