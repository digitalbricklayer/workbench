using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using Point = System.Windows.Point;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Visualizer for a grid.
    /// </summary>
    [Serializable]
    public class TableVisualizerModel : VisualizerModel
    {
        private readonly TableModel grid;

        /// <summary>
        /// Initialize a grid visualizer with a name, location, columns and rows.
        /// </summary>
        /// <param name="gridName">Grid name.</param>
        /// <param name="location">Grid location.</param>
        /// <param name="columnNames">Column names.</param>
        /// <param name="rows">Rows</param>
        public TableVisualizerModel(string gridName, Point location, string[] columnNames, TableRowModel[] rows)
            : base(gridName, location)
        {
            this.grid = new TableModel(columnNames, rows);
        }

        /// <summary>
        /// Initialize a grid visualizer with a name, location and grid model.
        /// </summary>
        /// <param name="gridName">Grid name.</param>
        /// <param name="location">Grid location.</param>
        /// <param name="gridModel">Grid model.</param>
        public TableVisualizerModel(string gridName, Point location, TableModel gridModel)
            : base(gridName, location)
        {
            Contract.Requires<ArgumentNullException>(gridModel != null);
            this.grid = gridModel;
        }

        /// <summary>
        /// Initialize a grid visualizer with a name and location.
        /// </summary>
        /// <param name="gridName">Grid name.</param>
        /// <param name="location">Grid location.</param>
        public TableVisualizerModel(string gridName, Point location)
            : base(gridName, location)
        {
            this.grid = new TableModel();
        }

        /// <summary>
        /// Gets the map model.
        /// </summary>
        public TableModel Grid
        {
            get { return this.grid; }
        }

        /// <summary>
        /// Update the grid visualizer with call arguments.
        /// </summary>
        /// <param name="theCall">Call arguments.</param>
        public override void UpdateWith(VisualizerCall theCall)
        {
            var rowIndex = theCall.GetArgumentByName("row");
            var columnIndex = theCall.GetArgumentByName("column");
            foreach (var callArgument in theCall.Arguments)
            {
                switch (callArgument.Name)
                {
                    case "BackgroundColor":
                    {
                        var backgroundColor = theCall.GetArgumentByName("BackgroundColor");
                        var theCell = this.grid.GetCellBy(Convert.ToInt32(rowIndex),
                                                          Convert.ToInt32(columnIndex));
                        switch (backgroundColor)
                        {
                            case "red":
                                theCell.BackgroundColor = Color.Red;
                                break;

                            case "green":
                                theCell.BackgroundColor = Color.Green;
                                break;

                            case "blue":
                                theCell.BackgroundColor = Color.Blue;
                                break;

                            default:
                                throw new NotImplementedException("Color not implemented.");
                        }
                        
                    }
                        break;

                    case "Text":
                    {
                        var theCell = this.grid.GetCellBy(Convert.ToInt32(rowIndex),
                                                          Convert.ToInt32(columnIndex));
                        theCell.Text = Convert.ToString(callArgument.Value);
                    }
                        break;

                    case "row":
                    case "column":
                        // No need to handle seperately, they are accessed directly by name.
                        break;

                    default:
                        throw new NotImplementedException("Unknown argument name.");
                }
            }
        }

        /// <summary>
        /// Get all rows in the grid.
        /// </summary>
        /// <returns>All rows in the grid.</returns>
        public IReadOnlyCollection<TableRowModel> GetRows()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<TableRowModel>>() != null);
            return Grid.GetRows();
        }

        /// <summary>
        /// Add a new column to the grid.
        /// </summary>
        /// <param name="theColumn">New column.</param>
        public void AddColumn(TableColumnModel theColumn)
        {
            Contract.Requires<ArgumentNullException>(theColumn != null);
            Grid.AddColumn(theColumn);
        }

        /// <summary>
        /// Add a new row to the grid.
        /// </summary>
        /// <param name="theRow">New row.</param>
        public void AddRow(TableRowModel theRow)
        {
            Contract.Requires<ArgumentNullException>(theRow != null);
            Grid.AddRow(theRow);
        }

        /// <summary>
        /// Get the column by name.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Returns the column with a name matching columnName.</returns>
        public TableColumnModel GetColumnByName(string columnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(columnName));
            return Grid.GetColumnByName(columnName);
        }

        public TableColumnData GetColumnDataByName(string columnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(columnName));
            return Grid.GetColumnDataByName(columnName);
        }
    }
}
