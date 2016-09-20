using System;
using System.Diagnostics.Contracts;
using System.Windows;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class VisualizerModel : GraphicModel
    {
        /// <summary>
        /// Initialize an unbound visualizer with a name and location.
        /// </summary>
        /// <param name="graphicName">Visualizer name.</param>
        /// <param name="location">Location.</param>
        protected VisualizerModel(string graphicName, Point location)
            : base(graphicName, location)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(graphicName));
        }

        /// <summary>
        /// Update the visualizer from the solution.
        /// </summary>
        /// <param name="theSnapshot">Solution snapshot.</param>
        public abstract void UpdateFrom(SolutionSnapshot theSnapshot);

        /// <summary>
        /// Update a visualizer with call arguments.
        /// </summary>
        /// <param name="theCall">Call arguments.</param>
        public abstract void UpdateWith(VisualizerCall theCall);
    }
}
