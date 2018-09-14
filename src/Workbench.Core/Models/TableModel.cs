using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A table model.
    /// </summary>
    [Serializable]
    public class TableModel : Model
    {
        private int columnCount, rowCount;
        private ObservableCollection<TableRowModel> rows;
        private ObservableCollection<TableColumnModel> columns;

        /// <summary>
        /// Initialize a table with columns and rows.
        /// </summary>
        /// <param name="theName">Table name.</param>
        /// <param name="columnNames">Column names.</param>
        /// <param name="theRows">Rows.</param>
        public TableModel(ModelName theName, string[] columnNames, TableRowModel[] theRows)
            : base(theName)
        {
            Rows = new ObservableCollection<TableRowModel>();
            Columns = new ObservableCollection<TableColumnModel>();

            foreach (var columnName in columnNames)
            {
                AppendColumn(new TableColumnModel(columnName));
            }
            this.columnCount = columnNames.Length;
            foreach (var row in theRows)
            {
                AppendRow(row);
            }
            this.rowCount = theRows.Length;
        }

        /// <summary>
        /// Initalize a table with empty rows and columns.
        /// </summary>
        public TableModel()
        {
            Rows = new ObservableCollection<TableRowModel>();
            Columns = new ObservableCollection<TableColumnModel>();
        }

        /// <summary>
        /// Gets the table rows.
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
        /// Gets the table columns.
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
        /// Gets a table with default configuration.
        /// </summary>
        public static TableModel Default
        {
            get
            {
                return new TableModel(new ModelName(), new[] { "X", "Y" }, new[] { new TableRowModel(), new TableRowModel() });
            }
        }

        /// <summary>
        /// Add a row to the table.
        /// </summary>
        /// <param name="selectedRowIndex"></param>
        /// <param name="theRow">The new row.</param>
        public void AddRowBefore(int selectedRowIndex, TableRowModel theRow)
        {
            Contract.Requires<ArgumentOutOfRangeException>(selectedRowIndex >= 0 && selectedRowIndex < Rows.Count);
            Contract.Requires<ArgumentNullException>(theRow != null);

            Rows.Insert(selectedRowIndex == 0 ? selectedRowIndex: selectedRowIndex - 1, theRow);
            this.rowCount++;
            if (theRow.Cells.Count == Columns.Count) return;
            for (var z = 0; z < Columns.Count; z++)
            {
                theRow.AddCell(new TableCellModel());
            }
        }

        /// <summary>
        /// Add a row to the table.
        /// </summary>
        /// <param name="selectedRowIndex"></param>
        /// <param name="theRow">The new row.</param>
        public void AddRowAfter(int selectedRowIndex, TableRowModel theRow)
        {
            Contract.Requires<ArgumentOutOfRangeException>(selectedRowIndex >= 0 && selectedRowIndex < Rows.Count);
            Contract.Requires<ArgumentNullException>(theRow != null);

            Rows.Insert(selectedRowIndex, theRow);
            this.rowCount++;
            if (theRow.Cells.Count == Columns.Count) return;
            for (var z = 0; z < Columns.Count; z++)
            {
                theRow.AddCell(new TableCellModel());
            }
        }

        /// <summary>
        /// Append a new row to the end of the table.
        /// </summary>
        /// <param name="theRow">The new row.</param>
        public void AddRow(TableRowModel theRow)
        {
            Rows.Add(theRow);
            this.rowCount++;
            if (theRow.Cells.Count == Columns.Count) return;
            for (var z = 0; z < Columns.Count; z++)
            {
                theRow.AddCell(new TableCellModel());
            }
        }

        /// <summary>
        /// Add a column to the table.
        /// </summary>
        /// <param name="selectedColumnIndex">The currently selected column index.</param>
        /// <param name="theColumn">The new column.</param>
        public void AddColumnBefore(int selectedColumnIndex, TableColumnModel theColumn)
        {
            Contract.Requires<ArgumentOutOfRangeException>(selectedColumnIndex >= 0 && selectedColumnIndex < Columns.Count);
            Contract.Requires<ArgumentNullException>(theColumn != null);

            // Column indexes start at 1
            theColumn.Index = selectedColumnIndex == 0? 1: selectedColumnIndex + 1;
            // Re-index the columns to the right of the inserted column
            for (var i = selectedColumnIndex; i < Columns.Count; i++)
            {
                Columns[i].Index++;
            }
            foreach (var row in Rows)
            {
                row.AddCell(new TableCellModel());
            }
            Columns.Insert(selectedColumnIndex, theColumn);
            this.columnCount++;
        }

        /// <summary>
        /// Add a column to the table.
        /// </summary>
        /// <param name="selectedColumnIndex">The currently selected column index.</param>
        /// <param name="theColumn">The new column.</param>
        public void AddColumnAfter(int selectedColumnIndex, TableColumnModel theColumn)
        {
            Contract.Requires<ArgumentOutOfRangeException>(selectedColumnIndex >= 0 && selectedColumnIndex < Columns.Count);
            Contract.Requires<ArgumentNullException>(theColumn != null);

            // Column indexes start at 1
            theColumn.Index = selectedColumnIndex == 0 ? 2 : selectedColumnIndex + 1;
            // Re-index the columns to the right of the inserted column
            for (var i = selectedColumnIndex; i < Columns.Count; i++)
            {
                Columns[i].Index++;
            }

            foreach (var row in Rows)
            {
                row.AddCell(new TableCellModel());
            }

            Columns.Insert(selectedColumnIndex + 1, theColumn);
            this.columnCount++;
        }

        /// <summary>
        /// Add a new column to the table.
        /// </summary>
        /// <param name="theColumn">The new column.</param>
        public void AddColumn(TableColumnModel theColumn)
        {
            Contract.Requires<ArgumentNullException>(theColumn != null);
            // Column indexes start at 1
            theColumn.Index = Columns.Count + 1;

            foreach (var row in Rows)
            {
                row.AddCell(new TableCellModel());
            }

            Columns.Add(theColumn);
            this.columnCount++;
        }

        /// <summary>
        /// Delete the currently selected row.
        /// </summary>
        /// <param name="selectedRowIndex">Selected row index.</param>
        public void DeleteRowSelected(int selectedRowIndex)
        {
            Contract.Requires<ArgumentOutOfRangeException>(selectedRowIndex >= 0 && selectedRowIndex < Rows.Count);

            Rows.RemoveAt(selectedRowIndex);
            this.rowCount--;
        }

        public void DeleteColumnSelected(int columnToDeleteIndex)
        {
            Columns.RemoveAt(columnToDeleteIndex);
            this.columnCount--;
            foreach (var row in Rows)
            {
                row.RemoveCell(columnToDeleteIndex);
            }
        }

        /// <summary>
        /// Get all rows in the table.
        /// </summary>
        /// <returns>All rows in the table.</returns>
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
            Contract.Requires<ArgumentOutOfRangeException>(rowIndex > 0 && rowIndex <= Rows.Count);
            Contract.Requires<ArgumentOutOfRangeException>(columnIndex > 0 && columnIndex <= Columns.Count);
            var row = this.rows[rowIndex - 1];
            return row.Cells[columnIndex - 1];
        }

        /// <summary>
        /// Get row at the row index.
        /// </summary>
        /// <param name="rowIndex">One based row index.</param>
        /// <returns>Row at the row index.</returns>
        public TableRowModel GetRowAt(int rowIndex)
        {
            Contract.Requires<ArgumentOutOfRangeException>(rowIndex > 0 && rowIndex <= Rows.Count);
            return Rows[rowIndex - 1];
        }

        /// <summary>
        /// Get column at the column index.
        /// </summary>
        /// <param name="columnIndex">One based column index.</param>
        /// <returns>Column at the column index.</returns>
        public TableColumnModel GetColumnAt(int columnIndex)
        {
            Contract.Requires<ArgumentOutOfRangeException>(columnIndex > 0 && columnIndex <= Columns.Count);
            return Columns[columnIndex - 1];
        }

        /// <summary>
        /// Resize the table.
        /// </summary>
        /// <param name="newColumnCount">Number of columns.</param>
        /// <param name="newRowCount">Number of rows.</param>
        public void Resize(int newColumnCount, int newRowCount)
        {
            if (newColumnCount > this.columnCount)
            {
                for (var i = this.columnCount; i < newColumnCount; i++)
                {
                    AppendColumn(new TableColumnModel(Convert.ToString(i)));
                }
            }

            if (newRowCount > this.rowCount)
            {
                for (var i = this.rowCount; i < newRowCount; i++)
                {
                    AppendRow(new TableRowModel());
                }
            }
        }

        private void AppendRow(TableRowModel theRow)
        {
            Rows.Add(theRow);
            this.rowCount++;
            if (theRow.Cells.Count == Columns.Count) return;
            for (var z = 0; z < Columns.Count; z++)
            {
                theRow.AddCell(new TableCellModel());
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

        private void AppendColumn(TableColumnModel theColumn)
        {
            // Column indexes start at 1
            theColumn.Index = this.columnCount + 1;
            foreach (var row in Rows)
            {
                row.AddCell(new TableCellModel());
            }
            Columns.Insert(this.columnCount, theColumn);
            this.columnCount++;
        }
    }
}
