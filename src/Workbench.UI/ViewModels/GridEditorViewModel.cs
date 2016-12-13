using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the grid editor dialog box.
    /// </summary>
    public sealed class GridEditorViewModel : Screen
    {
        private int rows;
        private int columns;

        /// <summary>
        /// Gets or sets the aggregate variable rows.
        /// </summary>
        public int Rows
        {
            get { return this.rows; }
            set
            {
                this.rows = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the aggregate variable rows.
        /// </summary>
        public int Columns
        {
            get { return this.columns; }
            set
            {
                this.columns = value;
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
