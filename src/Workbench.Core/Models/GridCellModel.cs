using System;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An individual cell in a row on a grid.
    /// </summary>
    [Serializable]
    public class GridCellModel : AbstractModel
    {
        private Color backgroundColor;
        private string text;

        /// <summary>
        /// Initialize the cell with text and color.
        /// </summary>
        /// <param name="theText">Region name.</param>
        /// <param name="theColor">Region color</param>
        public GridCellModel(string theText, Color theColor)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theText));
            Text = theText;
            BackgroundColor = theColor;
        }

        /// <summary>
        /// Initialize the cell with text.
        /// </summary>
        /// <param name="theText">Row text.</param>
        public GridCellModel(string theText)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theText));
            Text = theText;
            BackgroundColor = Color.White;
        }

        /// <summary>
        /// Initialize the cell with default values.
        /// </summary>
        public GridCellModel()
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
                Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(value));
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