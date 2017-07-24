using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows.Controls;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class GridViewModel : Screen
    {
        private readonly GridModel model;
        private ObservableCollection<DataGridColumn> columns;
        private ObservableCollection<DataGridRow> rows;

        public GridViewModel(GridModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.columns = new ObservableCollection<DataGridColumn>();
            this.rows = new ObservableCollection<DataGridRow>();
            this.model = theModel;
            PopulateGridColumns();
        }

        /// <summary>
        /// Gets the map model.
        /// </summary>
        public GridModel Grid
        {
            get { return this.model; }
        }

        /// <summary>
        /// Gets or sets the grid columns.
        /// </summary>
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return this.columns; }
            set
            {
                this.columns = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the grid rows.
        /// </summary>
        public ObservableCollection<DataGridRow> Rows
        {
            get { return this.rows; }
            set
            {
                this.rows = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddColumn(GridColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Grid.AddColumn(newColumn);
            Columns.Add(CreateDataGridColumnFrom(newColumn));
        }

        public void AddRow(GridRowModel theNewRow)
        {
            Contract.Requires<ArgumentNullException>(theNewRow != null);
            Grid.AddRow(theNewRow);
            Rows.Add(CreateDataGridRowFrom(theNewRow));
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

        private DataGridColumn CreateDataGridColumnFrom(GridColumnModel newColumn)
        {
            var newDataGridColumn = new DataGridTextColumn();
            newDataGridColumn.Header = newColumn.Name;
            return newDataGridColumn;
        }

        private DataGridRow CreateDataGridRowFrom(GridRowModel theNewRow)
        {
            return new DataGridRow();
        }

        private void PopulateGridColumns()
        {
            foreach (var column in Grid.Columns)
            {
                AddColumn(column);
            }
        }
    }
}
