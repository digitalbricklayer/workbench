using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Windows.Controls;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class GridViewModel : Screen
    {
        private readonly GridModel model;
        private DataTable dataTable;
        private DataGridCellInfo selectedCell;
        private object selectedItem;
        private int? selectedIndex;
        private object selectedColumn;

        public GridViewModel(GridModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.dataTable = CreateDataTable();
            this.model = theModel;
            PopulateGridColumns();
            PopulateGridRows();
            Contract.Assume(!this.dataTable.HasErrors);
            this.dataTable.AcceptChanges();
        }

        /// <summary>
        /// Gets the map model.
        /// </summary>
        public GridModel Grid
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

        public DataGridCellInfo SelectedCell
        {
            get { return this.selectedCell; }
            set
            {
                this.selectedCell = value;
                NotifyOfPropertyChange();
            }
        }

        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                this.selectedItem = value;
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

        public void AddColumn(GridColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Grid.AddColumn(newColumn);
            AddColumnToTable(newColumn);
        }

        public void AddRow(GridRowModel theNewRow)
        {
            Contract.Requires<ArgumentNullException>(theNewRow != null);
            Grid.AddRow(theNewRow);
            AddRowToTable(theNewRow);
        }

        public GridRowModel GetRowAt(int rowIndex)
        {
            return Grid.GetRowAt(rowIndex);
        }

        public void Resize(int newColumnCount, int newRowCount)
        {
            if (newColumnCount > Grid.Columns.Count)
            {
                for (var i = Grid.Columns.Count; i < newColumnCount; i++)
                {
                    AddColumn(new GridColumnModel(Convert.ToString(i)));
                }
            }
            Grid.Resize(newColumnCount, newRowCount);
        }

        private void PopulateGridColumns()
        {
            foreach (var column in Grid.Columns)
            {
                AddColumnToTable(column);
            }
        }

        private void PopulateGridRows()
        {
            foreach (var row in Grid.Rows)
            {
                AddRowToTable(row);
            }
        }

        private void AddColumnToTable(GridColumnModel newColumn)
        {
            this.dataTable.Columns.Add(newColumn.Name);
        }

        private void AddRowToTable(GridRowModel theRowModel)
        {
            var newRow = this.dataTable.NewRow();
            foreach (var column in Grid.Columns)
            {
                foreach (var row in theRowModel.Cells)
                    newRow[column.Name] = row.Text;
            }
            this.dataTable.Rows.Add(newRow);
        }

        private DataTable CreateDataTable()
        {
            var newTable = new DataTable();
            newTable.RowChanged += OnRowChanged;
            newTable.ColumnChanged += OnColumnChanged;
            return newTable;
        }

        private void OnColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
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
                case DataRowAction.Add:
                case DataRowAction.Change:
                    var selectedRow = Grid.GetRowAt(SelectedIndex.Value);
                    break;
            }
        }
    }
}
