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
        private int? selectedIndex;
        private object selectedColumn;

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

        public int? SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                NotifyOfPropertyChange();
            }
        }

        public object SelectedColumn
        {
            get { return selectedColumn; }
            set
            {
                this.selectedColumn = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddColumn(TableColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Table.AddColumn(newColumn);
            Data = CreateDataTable();
        }

        public void AddRow(TableRowModel theNewRow)
        {
            Contract.Requires<ArgumentNullException>(theNewRow != null);
            Table.AddRow(theNewRow);
            AddRowToTable(this.dataTable, theNewRow);
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
                    AddColumn(new TableColumnModel(Convert.ToString(i)));
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
                AddRowToTable(newTable, row);
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

        private void AddRowToTable(DataTable newTable, TableRowModel theRowModel)
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
        /// Update the grid model when the grid control is changed.
        /// </summary>
        /// <param name="sender">Data grid.</param>
        /// <param name="args">Row change event arguments.</param>
        private void OnRowChanged(object sender, DataRowChangeEventArgs args)
        {
            // Row may change before a row is selected
            if (SelectedIndex == null || SelectedIndex.Value == -1) return;

            switch (args.Action)
            {
                case DataRowAction.Change:
                    // Keep the model in sync with changes to the grid control
                    var selectedRow = Table.GetRowAt(SelectedIndex.Value);
                    selectedRow.UpdateCellsFrom(args.Row.ItemArray);
                    break;
            }
        }
    }
}
