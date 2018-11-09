using Workbench.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the tab name editor dialog box.
    /// </summary>
    public class TabNameEditorViewModel : DialogViewModel
    {
        private string _tabName;

        public TabNameEditorViewModel()
        {
            Validator = new TabNameValidator();
        }

        /// <summary>
        /// Gets or sets the tab title.
        /// </summary>
        public string TabName
        {
            get => this._tabName;
            set
            {
                _tabName = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
