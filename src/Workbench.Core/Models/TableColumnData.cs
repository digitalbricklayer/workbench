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
        private readonly IList<TableCellModel> cells;

        public TableColumnData(TableColumnModel theColumn, IEnumerable<TableCellModel> theColumnCells)
        {
            Contract.Requires<ArgumentException>(theColumn != null);
            Contract.Requires<ArgumentException>(theColumnCells != null);

            this.column = theColumn;
            this.cells = new List<TableCellModel>(theColumnCells);
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
                return new ReadOnlyCollection<TableCellModel>(this.cells);
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

        /// <summary>
        /// Get the cell at the one based index.
        /// </summary>
        /// <param name="cellIndex">One based index.</param>
        /// <returns>Cell at the index location.</returns>
        public TableCellModel GetCellAt(int cellIndex)
        {
#if WEIRD_COMPILER_ERROR
            // Fails with "Member 'Workbench.Core.Models.TableColumnData.cells' has less visibility than the enclosing method 'Workbench.Core.Models.TableColumnData.GetCellAt(System.Int32)'."
            Contract.Requires<ArgumentOutOfRangeException>(cellIndex > 0 && cellIndex <= this.cells.Count);
#endif
            return this.cells[cellIndex - 1];
        }
    }
}
