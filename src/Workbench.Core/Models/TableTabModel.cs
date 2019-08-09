using System;
using System.Collections.Generic;
using System.Drawing;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Visualizer for a grid.
    /// </summary>
    [Serializable]
    public class TableTabModel : VisualizerModel
    {
        private readonly TableModel _table;

        /// <summary>
        /// Initialize a table tab with a model and title.
        /// </summary>
        /// <param name="theTable">Table model.</param>
        /// <param name="theTitle">Table title.</param>
        public TableTabModel(TableModel theTable, WorkspaceTabTitle theTitle)
            : base(theTable, theTitle)
        {
            _table = theTable;
        }

        /// <summary>
        /// Gets the table model.
        /// </summary>
        public TableModel Table => _table;

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
                        var theCell = this._table.GetCellBy(Convert.ToInt32(rowIndex),
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
                        var theCell = this._table.GetCellBy(Convert.ToInt32(rowIndex),
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
            return Table.GetRows();
        }

        /// <summary>
        /// Get the column by name.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Returns the column with a name matching columnName.</returns>
        public TableColumnModel GetColumnByName(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException(nameof(columnName));

            return Table.GetColumnByName(columnName);
        }

        public TableColumnData GetColumnDataByName(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException(nameof(columnName));

            return Table.GetColumnDataByName(columnName);
        }


        /// <summary>
        /// Update a table with a property update.
        /// </summary>
        /// <param name="theUpdateContext">Property update context.</param>
        public override void UpdateWith(PropertyUpdateContext theUpdateContext)
        {
            Table.UpdateWith(theUpdateContext);
        }
    }
}
