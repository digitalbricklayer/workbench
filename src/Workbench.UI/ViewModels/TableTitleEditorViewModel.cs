using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the table title editor dialog box.
    /// </summary>
    public class TableTitleEditorViewModel : Screen
    {
        private string _tableTitle;

        /// <summary>
        /// Gets or sets the table title.
        /// </summary>
        public string TableTitle
        {
            get => this._tableTitle;
            set
            {
                _tableTitle = value;
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
