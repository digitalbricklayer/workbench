using Workbench.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the new tab details dialog.
    /// </summary>
    public class NewTabDetailsViewModel : DialogViewModel
    {
        private string _tabName;
        private string _tabDescription;

        /// <summary>
        /// Initialize a new tab details view model with default values.
        /// </summary>
        public NewTabDetailsViewModel()
        {
            TabName = string.Empty;
            TabDescription = string.Empty;
            Validator = new NewTabDetailsViewModelValidator();
        }

        /// <summary>
        /// Gets or sets the tab name.
        /// </summary>
        public string TabName
        {
            get => _tabName;
            set
            {
                _tabName = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the tab description.
        /// </summary>
        public string TabDescription
        {
            get => _tabDescription;
            set
            {
                _tabDescription = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
