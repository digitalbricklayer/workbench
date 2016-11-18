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
        private readonly IList<GridCellModel> cells;

        /// <summary>
        /// Initialize a grid column with a column name.
        /// </summary>
        /// <param name="theColumnName">Column name.</param>
        public GridColumnModel(string theColumnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theColumnName));
            this.cells = new List<GridCellModel>();
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
        /// Add a cell to the column.
        /// </summary>
        /// <param name="newCell">New cell.</param>
        public void AddCell(GridCellModel newCell)
        {
            Contract.Requires<ArgumentNullException>(newCell != null);
            this.cells.Add(newCell);
        }

        /// <summary>
        /// Get all cells in the column.
        /// </summary>
        /// <returns>All cells in the column.</returns>
        public IReadOnlyCollection<GridCellModel> GetCells()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<GridCellModel>>() != null);
            return new ReadOnlyCollection<GridCellModel>(this.cells);
        }
    }
}
