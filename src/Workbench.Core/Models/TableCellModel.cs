using System;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An individual cell in a row on a table.
    /// </summary>
    [Serializable]
    public class TableCellModel : AbstractModel
    {
        private Color backgroundColor;
        private string text;

        /// <summary>
        /// Initialize the cell with text.
        /// </summary>
        /// <param name="theText">Row text.</param>
        public TableCellModel(string theText)
        {
            Contract.Requires<ArgumentNullException>(theText != null);
            this.text = theText;
            BackgroundColor = Color.White;
        }

        /// <summary>
        /// Initialize the cell with default values.
        /// </summary>
        public TableCellModel()
        {
            BackgroundColor = Color.White;
            Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the row text.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.text = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the color of the region on the map.
        /// </summary>
        public Color BackgroundColor
        {
            get { return this.backgroundColor; }
            set
            {
                this.backgroundColor = value;
                OnPropertyChanged();
            }
        }
    }
}