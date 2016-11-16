using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A grid model.
    /// </summary>
    [Serializable]
    public class GridModel : AbstractModel
    {
        private ObservableCollection<GridRowModel> rows;
        private ObservableCollection<GridColumnModel> columns;

        /// <summary>
        /// Initialize the grid with an initial set of cells.
        /// </summary>
        /// <param name="theCells">Initial set of cells.</param>
        public GridModel(IEnumerable<GridRowModel> theCells)
        {
            Contract.Requires<ArgumentNullException>(theCells != null);
            this.rows = new ObservableCollection<GridRowModel>(theCells);
            this.columns = new ObservableCollection<GridColumnModel>();
        }

        /// <summary>
        /// Initialize the grid with an initial set of cells.
        /// </summary>
        /// <param name="theRows">Initial set of cells.</param>
        public GridModel(params GridRowModel[] theRows)
        {
            Contract.Requires<ArgumentNullException>(theRows != null);
            this.rows = new ObservableCollection<GridRowModel>(theRows);
            this.columns = new ObservableCollection<GridColumnModel>();
        }

        /// <summary>
        /// Initalize a grid with empty rows and columns.
        /// </summary>
        public GridModel()
        {
            this.rows = new ObservableCollection<GridRowModel>();
            this.columns = new ObservableCollection<GridColumnModel>();
        }

        /// <summary>
        /// Gets or sets the grid rows.
        /// </summary>
        public ObservableCollection<GridRowModel> Rows
        {
            get { return this.rows; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.rows = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the grid columns.
        /// </summary>
        public ObservableCollection<GridColumnModel> Columns
        {
            get { return this.columns; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.columns = value;
                OnPropertyChanged();
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
        }

        /// <summary>
        /// Add a column to the grid.
        /// </summary>
        /// <param name="theColumn">The new column.</param>
        public void AddColumn(GridRowModel theColumn)
        {
            Contract.Requires<ArgumentNullException>(theColumn != null);
            Rows.Add(theColumn);
        }
    }
}
