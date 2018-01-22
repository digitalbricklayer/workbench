using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    public class TableColumnData
    {
        private readonly TableColumnModel column;
        private readonly IReadOnlyCollection<TableCellModel> cells;

        public TableColumnData(TableColumnModel theColumn, IEnumerable<TableCellModel> theColumnCells)
        {
            Contract.Requires<ArgumentException>(theColumn != null);
            Contract.Requires<ArgumentException>(theColumnCells != null);

            this.column = theColumn;
            this.cells = new ReadOnlyCollection<TableCellModel>(theColumnCells.ToList());
        }

        /// <summary>
        /// Gets the column.
        /// </summary>
        public TableColumnModel Column
        {
            get
            {
                return this.column;
            }
        }

        /// <summary>
        /// Gets the cells in the column.
        /// </summary>
        public IReadOnlyCollection<TableCellModel> Cells
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
        public IReadOnlyCollection<TableCellModel> GetCells()
        {
            return new ReadOnlyCollection<TableCellModel>(this.cells.ToList());
        }
    }
}
