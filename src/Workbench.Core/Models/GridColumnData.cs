using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    public class GridColumnData
    {
        private readonly GridColumnModel column;
        private readonly IReadOnlyCollection<GridCellModel> cells;

        public GridColumnData(GridColumnModel theColumn, IEnumerable<GridCellModel> theColumnCells)
        {
            Contract.Requires<ArgumentException>(theColumn != null);
            Contract.Requires<ArgumentException>(theColumnCells != null);

            this.column = theColumn;
            this.cells = new ReadOnlyCollection<GridCellModel>(theColumnCells.ToList());
        }

        /// <summary>
        /// Gets the column.
        /// </summary>
        public GridColumnModel Column
        {
            get
            {
                return this.column;
            }
        }

        /// <summary>
        /// Gets the cells in the column.
        /// </summary>
        public IReadOnlyCollection<GridCellModel> Cells
        {
            get
            {
                return this.cells;
            }
        }

        /// <summary>
        /// Get the cells in the column.
        /// </summary>
        /// <returns>Cells in the column.</returns>
        public IReadOnlyCollection<GridCellModel> GetCells()
        {
            return new ReadOnlyCollection<GridCellModel>(this.cells.ToList());
        }
    }
}
