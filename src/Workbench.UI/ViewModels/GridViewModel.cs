using System;
using System.Data;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class GridViewModel : Screen
    {
        private readonly GridModel model;
        private DataTable dataTable;

        public GridViewModel(GridModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.dataTable = new DataTable();
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
    }
}
