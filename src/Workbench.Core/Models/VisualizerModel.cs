using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class VisualizerModel : GraphicModel
    {
        private string title;

        /// <summary>
        /// Initialize an unbound visualizer with a name and location.
        /// </summary>
        /// <param name="graphicName">Visualizers name.</param>
        /// <param name="location">Location.</param>
        protected VisualizerModel(string graphicName, Point location)
            : base(graphicName, location)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(graphicName));
        }

        /// <summary>
        /// Gets or sets the visualizer title.B
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Update a visualizer with call arguments.
        /// </summary>
        /// <param name="theCall">Call arguments.</param>
        public abstract void UpdateWith(VisualizerCall theCall);
    }
}
