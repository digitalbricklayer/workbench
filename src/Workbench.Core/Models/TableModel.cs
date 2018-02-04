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
    public class TableModel : Model
    {
        private int columnCount, rowCount;
        private ObservableCollection<TableRowModel> rows;
        private ObservableCollection<TableColumnModel> columns;

        /// <summary>
        /// Initialize a grid with columns and rows.
        /// </summary>
        /// <param name="columnNames">Column names.</param>
        /// <param name="theRows">Rows.</param>
        public TableModel(ModelName theName, string[] columnNames, TableRowModel[] theRows)
            : base(theName)
        {
            Rows = new ObservableCollection<TableRowModel>();
            Columns = new ObservableCollection<TableColumnModel>();

            foreach (var columnName in columnNames)
            {
                AddColumn(new TableColumnModel(columnName));
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
        public TableModel()
        {
            Rows = new ObservableCollection<TableRowModel>();
            Columns = new ObservableCollection<TableColumnModel>();
        }

        /// <summary>
        /// Gets the grid rows.
        /// </summary>
        public ObservableCollection<TableRowModel> Rows
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
        public ObservableCollection<TableColumnModel> Columns
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
        public static TableModel Default
        {
            get
            {
                return new TableModel(new ModelName(), new[] { "X", "Y" }, new[] { new TableRowModel(), new TableRowModel() });
            }
        }

        /// <summary>
        /// Add a row to the grid.
        /// </summary>
        /// <param name="theRow">The new row.</param>
        public void AddRow(TableRowModel theRow)
        {
            Contract.Requires<ArgumentNullException>(theRow != null);
            Rows.Add(theRow);
            if (theRow.Cells.Count == Columns.Count) return;
            for (var z = 0; z < Columns.Count; z++)
            {
                theRow.AddCell(new TableCellModel());
            }
        }

        /// <summary>
        /// Add a column to the grid.
        /// </summary>
        /// <param name="theColumn">The new column.</param>
        public void AddColumn(TableColumnModel theColumn)
        {
            Contract.Requires<ArgumentNullException>(theColumn != null);
            theColumn.Index = Columns.Count + 1;
            foreach (var row in Rows)
            {
                row.AddCell(new TableCellModel());
            }
            Columns.Add(theColumn);
        }

        /// <summary>
        /// Get all rows in the grid.
        /// </summary>
        /// <returns>All rows in the grid.</returns>
        public IReadOnlyCollection<TableRowModel> GetRows()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<TableRowModel>>() != null);
            return new ReadOnlyCollection<TableRowModel>(Rows);
        }

        /// <summary>
        /// Get the column by name.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Returns the column with a name matching columnName.</returns>
        public TableColumnModel GetColumnByName(string columnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(columnName));
            return this.columns.FirstOrDefault(_ => _.Name == columnName);
        }

        /// <summary>
        /// Get the column data by name.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Returns the column with a name matching columnName.</returns>
        public TableColumnData GetColumnDataByName(string columnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(columnName));
            var theColumn = GetColumnByName(columnName);
            return new TableColumnData(theColumn, GetCellsByColumn(theColumn));
        }

        /// <summary>
        /// Get the row by x, y co-ordinate.
        /// </summary>
        /// <param name="rowIndex">One based row index.</param>
        /// <param name="columnIndex">One based column index.</param>
        /// <returns>Cell matching the column and row indexes.</returns>
        public TableCellModel GetCellBy(int rowIndex, int columnIndex)
        {
            var row = this.rows[rowIndex - 1];
            return row.Cells[columnIndex - 1];
        }

        /// <summary>
        /// Get row at the row index.
        /// </summary>
        /// <param name="rowIndex">Row index.</param>
        /// <returns>Row at the row index.</returns>
        public TableRowModel GetRowAt(int rowIndex)
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
                    AddColumn(new TableColumnModel(Convert.ToString(i)));
                }
            }

            if (newRowCount > this.rowCount)
            {
                for (var i = this.rowCount; i < newRowCount; i++)
                {
                    AddRow(new TableRowModel());
                }
            }
        }

        private IEnumerable<TableCellModel> GetCellsByColumn(TableColumnModel theColumn)
        {
            var accumulator = new List<TableCellModel>();
            foreach (var row in Rows)
            {
                var x = row.GetCellAt(theColumn.Index - 1);
                accumulator.Add(x);
            }

            return accumulator;
        }
    }
}
