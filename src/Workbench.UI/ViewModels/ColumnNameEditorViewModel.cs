using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the column name editor dialog box.
    /// </summary>
    public class ColumnNameEditorViewModel : Screen
    {
        private string columnName;

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string ColumnName
        {
            get { return this.columnName; }
            set
            {
                this.columnName = value;
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
