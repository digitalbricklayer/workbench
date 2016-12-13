using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class GridVisualizerViewModel : VisualizerViewModel
    {
        public GridVisualizerViewModel(GridVisualizerDesignerViewModel theDesigner,
                                       GridVisualizerViewerViewModel theViewer)
        {
            Contract.Requires<ArgumentNullException>(theDesigner != null);
            Contract.Requires<ArgumentNullException>(theViewer != null);
            Designer = GridDesigner = theDesigner;
            Viewer = theViewer;
            Model = theViewer.MapModel;
        }

        public GridVisualizerModel Model { get; private set; }

        public GridVisualizerDesignerViewModel GridDesigner { get; private set; }

        /// <summary>
        /// Add a new column to the grid visializer.
        /// </summary>
        /// <param name="newColumn">New column.</param>
        public void AddColumn(GridColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Model.AddColumn(newColumn);
            GridDesigner.AddColumn(newColumn);
        }

        /// <summary>
        /// Resize the size of the grid.
        /// </summary>
        /// <param name="columns">Number of columns.</param>
        /// <param name="rows">Number of rows.</param>
        public void Resize(int columns, int rows)
        {
            GridDesigner.Resize(columns, rows);
        }
    }
}
