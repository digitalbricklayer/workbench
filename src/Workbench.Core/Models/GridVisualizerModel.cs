using System;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Visualizer for a grid.
    /// </summary>
    [Serializable]
    public class GridVisualizerModel : VisualizerModel
    {
        private readonly GridModel model;

        /// <summary>
        /// Initialize a grid visualizer with a name and location.
        /// </summary>
        /// <param name="gridName">Grid name.</param>
        /// <param name="location">Grid location.</param>
        public GridVisualizerModel(string gridName, Point location)
            : base(gridName, location)
        {
            this.model = new GridModel();
        }

        /// <summary>
        /// Gets the map model.
        /// </summary>
        public GridModel Model
        {
            get { return this.model; }
        }

        /// <summary>
        /// Update a visualizer with call arguments.
        /// </summary>
        /// <param name="theCall">Call arguments.</param>
        public override void UpdateWith(VisualizerCall theCall)
        {
        }
    }
}
