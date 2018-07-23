using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class TableVisualizerViewModel : VisualizerViewModel
    {
        public TableVisualizerViewModel(TableModel theTable, TableVisualizerEditorViewModel theEditor)
            : base(theTable, theEditor)
        {
            TableEditor = theEditor;
            Model = /*theViewer.GridModel*/ null;
        }

        public TableVisualizerModel Model { get; private set; }

        /// <summary>
        /// Gets the table editor.
        /// </summary>
        public TableVisualizerEditorViewModel TableEditor { get; private set; }

        /// <summary>
        /// Add a new column to the grid visializer.
        /// </summary>
        /// <param name="newColumn">New column.</param>
        public void AddColumn(TableColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            TableEditor.AddColumn(newColumn);
        }

        /// <summary>
        /// Add a new row to the grid visualizer.
        /// </summary>
        /// <param name="newRow">New row.</param>
        public void AddRow(TableRowModel newRow)
        {
            Contract.Requires<ArgumentNullException>(newRow != null);
            TableEditor.AddRow(newRow);
        }

        /// <summary>
        /// Resize the size of the grid.
        /// </summary>
        /// <param name="columns">Number of columns.</param>
        /// <param name="rows">Number of rows.</param>
        public void Resize(int columns, int rows)
        {
            TableEditor.Resize(columns, rows);
        }

        /// <summary>
        /// Delete the selected column.
        /// </summary>
        public void DeleteColumnSelected()
        {
            TableEditor.DeleteColumnSelected();
        }

        /// <summary>
        /// Delete the selected row.
        /// </summary>
        public void DeleteRowSelected()
        {
            TableEditor.DeleteRowSelected();
        }
    }
}
