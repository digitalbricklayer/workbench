using Caliburn.Micro;

namespace Workbench.ViewModels
{
    public class DomainEditViewModel : Screen
    {
        private string _domainName;
        private string _domainExpression;

        /// <summary>
        /// Initialize the domain edit with default values.
        /// </summary>
        public DomainEditViewModel()
        {
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

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void AcceptButton()
        {
            TryClose(true);
        }
    }
}
