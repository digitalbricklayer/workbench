using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A column inside a table.
    /// </summary>
    [Serializable]
    public class TableColumnModel : AbstractModel
    {
        private string name;
        private int index;

        /// <summary>
        /// Initialize a table column with a column name.
        /// </summary>
        /// <param name="theColumnName">Column name.</param>
        public TableColumnModel(string theColumnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theColumnName));
            Name = theColumnName;
        }

        /// <summary>
        /// Gets or sets the table column name.
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
        /// Gets or sets the table column index.
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
