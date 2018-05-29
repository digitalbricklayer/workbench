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
        private readonly IConductor parent;

        public TableViewModel(TableModel theModel, IConductor theParent)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theParent != null);

            this.model = theModel;
            this.parent = theParent;
            this.dataTable = CreateDataTable();
            Contract.Assert(!this.dataTable.HasErrors);
        }

        /// <summary>
        /// Gets the map model.
        /// </summary>
        public TableModel Grid
        {
            get { return this.model; }
        }

        /// <summary>
        /// Gets or sets the data table for the grid.
        /// </summary>
        public DataTable Table
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
            Grid.AddColumn(newColumn);
            Table = CreateDataTable();
        }

        public void AddRow(TableRowModel theNewRow)
        {
            Contract.Requires<ArgumentNullException>(theNewRow != null);
            Grid.AddRow(theNewRow);
            AddRowToTable(this.dataTable, theNewRow);
        }

        public TableRowModel GetRowAt(int rowIndex)
        {
            return Grid.GetRowAt(rowIndex);
        }

        public void Resize(int newColumnCount, int newRowCount)
        {
            if (newColumnCount > Grid.Columns.Count)
            {
                for (var i = Grid.Columns.Count; i < newColumnCount; i++)
                {
                    AddColumn(new TableColumnModel(Convert.ToString(i)));
                }
            }
            Grid.Resize(newColumnCount, newRowCount);
        }

        public void DeleteColumnSelected()
        {
            Contract.Assume(SelectedColumn != null);
//            DeleteRowFromTable(SelectedColumn);
        }

        public void OnGridFocus()
        {
            var x = (GraphicViewModel) this.parent;
            x.IsSelected = true;
        }

        private void PopulateGridColumns(DataTable newTable)
        {
            foreach (var column in Grid.Columns)
            {
                AddColumnToTable(newTable, column);
            }
        }

        private void PopulateGridRows(DataTable newTable)
        {
            foreach (var row in Grid.Rows)
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
            PopulateGridColumns(newTable);
            PopulateGridRows(newTable);
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
                    var selectedRow = Grid.GetRowAt(SelectedIndex.Value);
                    selectedRow.UpdateCellsFrom(args.Row.ItemArray);
                    break;
            }
        }
    }
}
