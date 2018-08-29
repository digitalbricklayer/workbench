using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the tab title editor dialog box.
    /// </summary>
    public class TabTitleEditorViewModel : Screen
    {
        private string _tabTitle;

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

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void AcceptButton()
        {
            TryClose(true);
        }
    }
}
