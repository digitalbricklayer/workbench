using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    public class TableColumnData
    {
        private readonly TableColumnModel _column;
        private readonly IList<TableCellModel> _cells;

        public TableColumnData(TableColumnModel theColumn, IEnumerable<TableCellModel> theColumnCells)
        {
            Contract.Requires<ArgumentException>(theColumn != null);
            Contract.Requires<ArgumentException>(theColumnCells != null);

            _column = theColumn;
            _cells = new List<TableCellModel>(theColumnCells);
        }

        /// <summary>
        /// Gets the column.
        /// </summary>
        public TableColumnModel Column => _column;

        /// <summary>
        /// Gets the cells in the column.
        /// </summary>
        public IReadOnlyCollection<TableCellModel> Cells => new ReadOnlyCollection<TableCellModel>(_cells);

        /// <summary>
        /// Get the cells in the column.
        /// </summary>
        /// <returns>Cells in the column.</returns>
        public IReadOnlyCollection<TableCellModel> GetCells()
        {
            return new ReadOnlyCollection<TableCellModel>(_cells.ToList());
        }

        /// <summary>
        /// Get the cell at the one based index.
        /// </summary>
        /// <param name="cellIndex">One based index.</param>
        /// <returns>Cell at the index location.</returns>
        public TableCellModel GetCellAt(int cellIndex)
        {
#if WEIRD_COMPILER_ERROR_FIXED
            // Fails with "Member 'Workbench.Core.Models.TableColumnData.cells' has less visibility than the enclosing method 'Workbench.Core.Models.TableColumnData.GetCellAt(System.Int32)'."
            Contract.Requires<ArgumentOutOfRangeException>(cellIndex > 0 && cellIndex <= _cells.Count);
#endif
            return _cells[cellIndex - 1];
        }
    }
}
