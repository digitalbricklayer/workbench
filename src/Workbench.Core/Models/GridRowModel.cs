using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

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
                AddCell(new GridCellModel(cellContent));
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

        public void AddCell(GridCellModel newCell)
        {
            this.cells.Add(newCell);
        }

        public IReadOnlyCollection<GridCellModel> GetCells()
        {
            return new ReadOnlyCollection<GridCellModel>(this.cells);
        }

        public IReadOnlyCollection<object> GetCellContent()
        {
            var accumulator = Cells.Select(aCell => aCell.Text)
                                   .Cast<object>()
                                   .ToList();

            return accumulator.AsReadOnly();
        }

        public GridCellModel GetCellAt(int index)
        {
            return this.cells[index];
        }

        public void UpdateCellsFrom(object[] rowItems)
        {
            var i = 0;
            foreach (var item in rowItems)
            {
                this.cells[i++].Text = Convert.ToString(item);
            }
        }
    }
}
