using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Visualizer for a grid.
    /// </summary>
    [Serializable]
    public class TableVisualizerModel : VisualizerModel
    {
        private readonly TableModel table;

        /// <summary>
        /// Initialize a grid visualizer with a name, location, columns and rows.
        /// </summary>
        /// <param name="theTitle">Grid name.</param>
        /// <param name="location">Grid location.</param>
        /// <param name="columnNames">Column names.</param>
        /// <param name="rows">Rows</param>
        public TableVisualizerModel(TableModel theTable, VisualizerTitle theTitle, System.Windows.Point location)
            : base(theTable, theTitle, location)
        {
            this.table = theTable;
        }

        /// <summary>
        /// Gets the table model.
        /// </summary>
        public TableModel Table
        {
            get { return this.table; }
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
                        var theCell = this.table.GetCellBy(Convert.ToInt32(rowIndex),
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
                        var theCell = this.table.GetCellBy(Convert.ToInt32(rowIndex),
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
            return Table.GetRows();
        }

        /// <summary>
        /// Add a new column to the grid.
        /// </summary>
        /// <param name="theColumn">New column.</param>
        public void AddColumn(TableColumnModel theColumn)
        {
            Contract.Requires<ArgumentNullException>(theColumn != null);
            Table.AddColumn(theColumn);
        }

        /// <summary>
        /// Add a new row to the grid.
        /// </summary>
        /// <param name="theRow">New row.</param>
        public void AddRow(TableRowModel theRow)
        {
            Contract.Requires<ArgumentNullException>(theRow != null);
            Table.AddRow(theRow);
        }

        /// <summary>
        /// Get the column by name.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Returns the column with a name matching columnName.</returns>
        public TableColumnModel GetColumnByName(string columnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(columnName));
            return Table.GetColumnByName(columnName);
        }

        public TableColumnData GetColumnDataByName(string columnName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(columnName));
            return Table.GetColumnDataByName(columnName);
        }
    }
}
