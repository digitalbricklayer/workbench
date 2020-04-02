using Workbench.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Edit the name of a bundle.
    /// </summary>
    public class BundleNameEditorViewModel : DialogViewModel
    {
        private string _bundleName;

        /// <summary>
        /// Initialize the bundle name editor with default values.
        /// </summary>
        public BundleNameEditorViewModel()
        {
            Validator = new BundleNameEditorViewModelValidator();
            BundleName = string.Empty;
        }

        /// <summary>
        /// Gets or sets the bundle name.
        /// </summary>
        public string BundleName
        {
            get => _bundleName;
            set
            {
                _bundleName = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
