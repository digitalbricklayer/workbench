using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Model for the variable visualizer.
    /// </summary>
    [Serializable]
    public class VariableVisualizerModel : VisualizerModel
    {
        /// <summary>
        /// Initialize the variable visualizer with the location and bound variable.
        /// </summary>
        /// <param name="newLocation">The graphic location.</param>
        /// <param name="theBoundVariable">The variable bound to the visualizer.</param>
        public VariableVisualizerModel(Point newLocation, VariableModel theBoundVariable)
            : base("A visualizer", newLocation, theBoundVariable)
        {
            Contract.Requires<ArgumentNullException>(theBoundVariable != null);
        }

        /// <summary>
        /// Initialize the variable visualizer with the location.
        /// </summary>
        /// <param name="newLocation">The graphic location.</param>
        public VariableVisualizerModel(Point newLocation)
            : base("A visualizer", newLocation)
        {
        }
    }
}
