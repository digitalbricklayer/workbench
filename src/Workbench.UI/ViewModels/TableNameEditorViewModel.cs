using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the table name editor dialog box.
    /// </summary>
    public class TableNameEditorViewModel : Screen
    {
        private string _tableName;

        /// <summary>
        /// Gets or sets the table title.
        /// </summary>
        public string TableName
        {
            get => this._tableName;
            set
            {
                _tableName = value;
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
