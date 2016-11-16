using System;
using System.Diagnostics.Contracts;
using System.Windows.Media;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An individual region on a map.
    /// </summary>
    [Serializable]
    public class GridCellModel : AbstractModel
    {
        private System.Drawing.Color backgroundColor;
        private string name;
        private PointCollection boundary;

        /// <summary>
        /// Initialize the region with a name and color.
        /// </summary>
        /// <param name="theName">Region name.</param>
        /// <param name="theColor">Region color</param>
        public GridCellModel(string theName, System.Drawing.Color theColor)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));
            Name = theName;
            BackgroundColor = theColor;
        }

        /// <summary>
        /// Initialize the region with a name.
        /// </summary>
        /// <param name="theName">Region name.</param>
        public GridCellModel(string theName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));
            Name = theName;
            BackgroundColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// Initialize the region with default values.
        /// </summary>
        public GridCellModel()
        {
            BackgroundColor = System.Drawing.Color.White;
            Name = String.Empty;
        }

        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(value));
                this.name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the color of the region on the map.
        /// </summary>
        public System.Drawing.Color BackgroundColor
        {
            get { return this.backgroundColor; }
            set
            {
                this.backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public PointCollection Boundary
        {
            get { return this.boundary; }
            set
            {
                this.boundary = value;
                OnPropertyChanged();
            }
        }
    }
}
