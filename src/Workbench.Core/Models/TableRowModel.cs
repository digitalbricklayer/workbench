using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An row on a grid.
    /// </summary>
    [Serializable]
    public class TableRowModel : AbstractModel
    {
        private ObservableCollection<TableCellModel> cells;

        /// <summary>
        /// Initialize a row with cell data.
        /// </summary>
        /// <param name="cellData">Cell data.</param>
        public TableRowModel(params string[] cellData)
            : this()
        {
            foreach (var cellContent in cellData)
            {
                AddCell(new TableCellModel(cellContent));
            }
        }

        /// <summary>
        /// Initialize a row with cells.
        /// </summary>
        /// <param name="cellData">Cells to insert into the row.</param>
        public TableRowModel(params TableCellModel[] cellData)
            : this()
        {
            foreach (var cellContent in cellData)
            {
                AddCell(cellContent);
            }
        }

        /// <summary>
        /// Initialize a row with default values.
        /// </summary>
        public TableRowModel()
        {
            this.cells = new ObservableCollection<TableCellModel>();
        }

        /// <summary>
        /// Gets or sets the row cells.
        /// </summary>
        public ObservableCollection<TableCellModel> Cells
        {
            get { return this.cells; }
            set
            {
                this.cells = value;
                OnPropertyChanged();
            }
        }

        public void AddCell(TableCellModel newCell)
        {
            this.cells.Add(newCell);
        }

        public IReadOnlyCollection<TableCellModel> GetCells()
        {
            return new ReadOnlyCollection<TableCellModel>(this.cells);
        }

        public IReadOnlyCollection<object> GetCellContent()
        {
            var accumulator = Cells.Select(aCell => aCell.Text)
                                   .Cast<object>()
                                   .ToList();

            return accumulator.AsReadOnly();
        }

        /// <summary>
        /// Get the cell at the index.
        /// </summary>
        /// <param name="index">One based column index.</param>
        /// <returns>Cell located at the column index.</returns>
        public TableCellModel GetCellAt(int index)
        {
            return this.cells[index - 1];
        }

        public void UpdateCellsFrom(object[] rowItems)
        {
            var i = 0;
            foreach (var item in rowItems)
            {
                this.cells[i++].Text = Convert.ToString(item);
            }
        }

        public void RemoveCell(int columnToDeleteIndex)
        {
            this.cells.RemoveAt(columnToDeleteIndex);
        }
    }
}
