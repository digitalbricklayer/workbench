using System;
using System.Collections.ObjectModel;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An row on a grid.
    /// </summary>
    [Serializable]
    public class GridRowModel : AbstractModel
    {
        private ObservableCollection<GridCellModel> cells;

        /// <summary>
        /// Initialize a row with cell data.
        /// </summary>
        /// <param name="cellData">Cell data.</param>
        public GridRowModel(params string[] cellData)
            : this()
        {
            foreach (var cellContent in cellData)
            {
                this.cells.Add(new GridCellModel(cellContent));
            }
        }

        /// <summary>
        /// Initialize a row with default values.
        /// </summary>
        public GridRowModel()
        {
            this.cells = new ObservableCollection<GridCellModel>();
        }

        /// <summary>
        /// Gets or sets the row cells.
        /// </summary>
        public ObservableCollection<GridCellModel> Cells
        {
            get { return this.cells; }
            set
            {
                this.cells = value;
                OnPropertyChanged();
            }
        }
    }
}