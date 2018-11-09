using Workbench.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the tab title editor dialog box.
    /// </summary>
    public class TabTitleEditorViewModel : DialogViewModel
    {
        private string _tabTitle;

        public TabTitleEditorViewModel()
        {
            Validator = new TabTitleEditorValidator();
        }

        /// <summary>
        /// Gets or sets the tab title.
        /// </summary>
        public string TabTitle
        {
            get => this._tabTitle;
            set
            {
                _tabTitle = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
