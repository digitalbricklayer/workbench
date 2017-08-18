using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A column inside a grid.
    /// </summary>
    [Serializable]
    public class GridColumnModel : AbstractModel
    {
        private string name;
        private int index;

        /// <summary>
        /// Initialize a grid column with a column name.
        /// </summary>
        /// <param name="theColumnName">Column name.</param>
        public GridColumnModel(string theColumnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theColumnName));
            Name = theColumnName;
        }

        /// <summary>
        /// Gets or sets the grid column name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the grid column index.
        /// </summary>
        public int Index
        {
            get { return this.index; }
            internal set
            {
                this.index = value;
                OnPropertyChanged();
            }
        }
    }
}
