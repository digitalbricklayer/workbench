using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A grid model.
    /// </summary>
    [Serializable]
    public class GridModel : AbstractModel
    {
        private int columnCount, rowCount;
        private ObservableCollection<GridRowModel> rows;
        private ObservableCollection<GridColumnModel> columns;

        /// <summary>
        /// Initialize a grid with columns and rows.
        /// </summary>
        /// <param name="columnNames">Column names.</param>
        /// <param name="theRows">Rows.</param>
        public GridModel(string[] columnNames, GridRowModel[] theRows)
            : this()
        {
            foreach (var columnName in columnNames)
            {
                AddColumn(new GridColumnModel(columnName));
            }
            this.columnCount = columnNames.Length;
            foreach (var row in theRows)
            {
                AddRow(row);
            }
            this.rowCount = theRows.Length;
        }

        /// <summary>
        /// Initalize a grid with empty rows and columns.
        /// </summary>
        public GridModel()
        {
            Rows = new ObservableCollection<GridRowModel>();
            Columns = new ObservableCollection<GridColumnModel>();
        }

        /// <summary>
        /// Gets the grid rows.
        /// </summary>
        public ObservableCollection<GridRowModel> Rows
        {
            get { return this.rows; }
            private set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.rows = value;
            }
        }

        /// <summary>
        /// Gets the grid columns.
        /// </summary>
        public ObservableCollection<GridColumnModel> Columns
        {
            get { return this.columns; }
            private set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.columns = value;
            }
        }

        /// <summary>
        /// Gets the grid as it is displayed when newly created.
        /// </summary>
        public static GridModel Default
        {
            get
            {
                return new GridModel(new[] { "X", "Y" }, new[] { new GridRowModel(), new GridRowModel() });
            }
        }

        /// <summary>
        /// Add a row to the grid.
        /// </summary>
        /// <param name="theRow">The new row.</param>
        public void AddRow(GridRowModel theRow)
        {
            Contract.Requires<ArgumentNullException>(theRow != null);
            Rows.Add(theRow);

#if false
//            if (theRow.GetCells().Count == Columns.Count) return;

            var i = 0;
            /*
             * Cells are stored inside the row in the same order as the 
             * columns appear in the grid.
             */
            foreach (var cell in theRow.Cells)
            {
                var theColumn = this.columns[i];
                theColumn.AddCell(cell);
                i++;
            }
            for (var z = 0; z < Columns.Count; z++)
            {
                theRow.AddCell(new GridCellModel());
            }
#else
            if (theRow.Cells.Count == Columns.Count) return;
            for (var z = 0; z < Columns.Count; z++)
            {
                theRow.AddCell(new GridCellModel());
            }
#endif
        }

        /// <summary>
        /// Add a column to the grid.
        /// </summary>
        /// <param name="theColumn">The new column.</param>
        public void AddColumn(GridColumnModel theColumn)
        {
            Contract.Requires<ArgumentNullException>(theColumn != null);
            theColumn.Index = Columns.Count + 1;
            foreach (var row in Rows)
            {
                row.AddCell(new GridCellModel());
            }
            Columns.Add(theColumn);
        }

        /// <summary>
        /// Get all rows in the grid.
        /// </summary>
        /// <returns>All rows in the grid.</returns>
        public IReadOnlyCollection<GridRowModel> GetRows()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<GridRowModel>>() != null);
            return new ReadOnlyCollection<GridRowModel>(Rows);
        }

        /// <summary>
        /// Get the column by name.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Returns the column with a name matching columnName.</returns>
        public GridColumnModel GetColumnByName(string columnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(columnName));
            return this.columns.FirstOrDefault(_ => _.Name == columnName);
        }

        /// <summary>
        /// Get the column data by name.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Returns the column with a name matching columnName.</returns>
        public GridColumnData GetColumnDataByName(string columnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(columnName));
            var theColumn = GetColumnByName(columnName);
            return new GridColumnData(theColumn, GetCellsByColumn(theColumn));
        }

        /// <summary>
        /// Get the row by x, y co-ordinate.
        /// </summary>
        /// <param name="rowIndex">One based row index.</param>
        /// <param name="columnIndex">One based column index.</param>
        /// <returns>Cell matching the column and row indexes.</returns>
        public GridCellModel GetCellBy(int rowIndex, int columnIndex)
        {
            var row = this.rows[rowIndex - 1];
            return row.Cells[columnIndex - 1];
        }

        /// <summary>
        /// Get row at the row index.
        /// </summary>
        /// <param name="rowIndex">Row index.</param>
        /// <returns>Row at the row index.</returns>
        public GridRowModel GetRowAt(int rowIndex)
        {
            return Rows[rowIndex];
        }

        /// <summary>
        /// Resize the grid.
        /// </summary>
        /// <param name="newColumnCount">Number of columns.</param>
        /// <param name="newRowCount">Number of rows.</param>
        public void Resize(int newColumnCount, int newRowCount)
        {
            if (newColumnCount > this.columnCount)
            {
                for (var i = this.columnCount; i < newColumnCount; i++)
                {
                    AddColumn(new GridColumnModel(Convert.ToString(i)));
                }
            }

            if (newRowCount > this.rowCount)
            {
                for (var i = this.rowCount; i < newRowCount; i++)
                {
                    AddRow(new GridRowModel());
                }
            }
        }

        private IEnumerable<GridCellModel> GetCellsByColumn(GridColumnModel theColumn)
        {
            var accumulator = new List<GridCellModel>();
            foreach (var row in Rows)
            {
                var x = row.GetCellAt(theColumn.Index - 1);
                accumulator.Add(x);
            }

            return accumulator;
        }
    }
}
