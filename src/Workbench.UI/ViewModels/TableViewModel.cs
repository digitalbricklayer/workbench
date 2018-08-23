using System;
using System.Data;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class TableViewModel : Screen
    {
        private readonly TableModel model;
        private DataTable dataTable;
        private int? _selectedRow;
        private int? _selectedColumn;

        public TableViewModel(TableModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            this.model = theModel;
            this.dataTable = CreateDataTable();
            Contract.Assert(!this.dataTable.HasErrors);
        }

        /// <summary>
        /// Gets the table model.
        /// </summary>
        public TableModel Table
        {
            get { return this.model; }
        }

        /// <summary>
        /// Gets or sets the data table.
        /// </summary>
        public DataTable Data
        {
            get
            {
                return this.dataTable;
            }
            set
            {
                this.dataTable = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the selected row.
        /// </summary>
        public int? SelectedRow
        {
            get => _selectedRow;
            set
            {
                _selectedRow = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the selected column.
        /// </summary>
        public int? SelectedColumn
        {
            get => _selectedColumn;
            set
            {
                _selectedColumn = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddColumnAfter(int selectedColumnIndex, TableColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Table.AddColumnAfter(selectedColumnIndex, newColumn);
            Data = CreateDataTable();
        }

        public void AddColumnBefore(int selectedColumnIndex, TableColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Table.AddColumnBefore(selectedColumnIndex, newColumn);
            Data = CreateDataTable();
        }

        public void AddRowBefore(int selectedRowIndex, TableRowModel theNewRow)
        {
            Contract.Requires<ArgumentNullException>(theNewRow != null);
            Table.AddRowBefore(selectedRowIndex, theNewRow);
            AddRowToTable(theNewRow, selectedRowIndex);
        }

        public void AddRowAfter(int selectedRowIndex, TableRowModel theNewRow)
        {
            Contract.Requires<ArgumentNullException>(theNewRow != null);
            Table.AddRowAfter(selectedRowIndex, theNewRow);
            AddRowToTable(theNewRow, selectedRowIndex);
        }

        public TableRowModel GetRowAt(int rowIndex)
        {
            return Table.GetRowAt(rowIndex);
        }

        public void Resize(int newColumnCount, int newRowCount)
        {
            if (newColumnCount > Table.Columns.Count)
            {
                for (var i = Table.Columns.Count; i < newColumnCount; i++)
                {
                    AddColumnEnd(new TableColumnModel(Convert.ToString(i)));
                }
            }
            Table.Resize(newColumnCount, newRowCount);
        }

        public void DeleteColumnSelected()
        {
            Contract.Assume(SelectedColumn != null);
//            DeleteRowFromTable(SelectedColumn);
        }

        private void PopulateTableColumns(DataTable newTable)
        {
            foreach (var column in Table.Columns)
            {
                AddColumnToTable(newTable, column);
            }
        }

        private void PopulateTableRows(DataTable newTable)
        {
            foreach (var row in Table.Rows)
            {
                AppendRowToTable(row, newTable);
            }
        }

        private void AddColumnToTable(DataTable newTable, TableColumnModel newColumn)
        {
            var newTableColumn = new DataColumn(newColumn.Name, typeof(string));
            newTableColumn.DefaultValue = string.Empty;
            newTable.Columns.Add(newTableColumn);
            newTable.AcceptChanges();
            Contract.Assert(!newTable.HasErrors);
        }

        private void AddColumnEnd(TableColumnModel newColumn)
        {
            throw new NotImplementedException();
        }

        private void AddRowToTable(TableRowModel theRowModel, int selectedRowIndex)
        {
            var newRow = this.dataTable.NewRow();
            var i = 0;
            foreach (var row in theRowModel.Cells)
                newRow[i++] = row.Text;
            this.dataTable.Rows.InsertAt(newRow, selectedRowIndex);
            this.dataTable.AcceptChanges();
        }

        private void AppendRowToTable(TableRowModel theRowModel, DataTable newTable)
        {
            var newRow = newTable.NewRow();
            var i = 0;
            foreach (var row in theRowModel.Cells)
                newRow[i++] = row.Text;
            newTable.Rows.Add(newRow);
            newTable.AcceptChanges();
        }

        private DataTable CreateDataTable()
        {
            var newTable = new DataTable();
            newTable.RowChanged += OnRowChanged;
            PopulateTableColumns(newTable);
            PopulateTableRows(newTable);
            newTable.AcceptChanges();
            return newTable;
        }

        /// <summary>
        /// Update the table model when the grid control is changed.
        /// </summary>
        /// <param name="sender">Data grid.</param>
        /// <param name="args">Row change event arguments.</param>
        private void OnRowChanged(object sender, DataRowChangeEventArgs args)
        {
            // Row may change before a row is selected
            if (SelectedRow == null || SelectedRow == -1) return;

            switch (args.Action)
            {
                case DataRowAction.Change:
                    // Keep the model in sync with changes to the grid
                    var selectedRow = Table.GetRowAt(SelectedRow.Value);
                    selectedRow.UpdateCellsFrom(args.Row.ItemArray);
                    break;
            }
        }
    }
}
