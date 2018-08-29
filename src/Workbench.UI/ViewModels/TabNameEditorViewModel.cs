using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the tab name editor dialog box.
    /// </summary>
    public class TabNameEditorViewModel : Screen
    {
        private string _tabName;

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

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void AcceptButton()
        {
            TryClose(true);
        }
    }
}
