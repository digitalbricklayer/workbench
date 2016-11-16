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
        private ObservableCollection<GridCellModel> cells;

        /// <summary>
        /// Initialize the grid with an initial set of cells.
        /// </summary>
        /// <param name="theCells">Initial set of cells.</param>
        public GridModel(IEnumerable<GridCellModel> theCells)
        {
            Contract.Requires<ArgumentNullException>(theCells != null);
            this.cells = new ObservableCollection<GridCellModel>(theCells);
        }

        /// <summary>
        /// Initialize the grid with an initial set of cells.
        /// </summary>
        /// <param name="theCells">Initial set of cells.</param>
        public GridModel(params GridCellModel[] theCells)
        {
            Contract.Requires<ArgumentNullException>(theCells != null);
            this.cells = new ObservableCollection<GridCellModel>(theCells);
        }

        /// <summary>
        /// Initalize a grid with a default set of cells.
        /// </summary>
        public GridModel()
        {
            this.cells = new ObservableCollection<GridCellModel>();
        }

        /// <summary>
        /// Gets or sets the grid cells.
        /// </summary>
        public ObservableCollection<GridCellModel> Cells
        {
            get { return this.cells; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.cells = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Add a cell to the map.
        /// </summary>
        /// <param name="theCell">The new cell.</param>
        public void AddCell(GridCellModel theCell)
        {
            Contract.Requires<ArgumentNullException>(theCell != null);
            Cells.Add(theCell);
        }
    }
}
