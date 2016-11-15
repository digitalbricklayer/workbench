using System;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Visualizer for a map.
    /// </summary>
    [Serializable]
    public class MapVisualizerModel : VisualizerModel
    {
        private readonly MapModel model;

        /// <summary>
        /// Initialize a map visualizer with a name and location.
        /// </summary>
        /// <param name="mapName">Map name.</param>
        /// <param name="location">Map location.</param>
        public MapVisualizerModel(string mapName, Point location)
            : base(mapName, location)
        {
            this.model = new MapModel();
        }

        /// <summary>
        /// Gets the map model.
        /// </summary>
        public MapModel Model
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
