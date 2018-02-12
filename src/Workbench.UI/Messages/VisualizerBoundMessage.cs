using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a visualizer is bound to a variable.
    /// </summary>
    public class VisualizerBoundMessage
    {
        /// <summary>
        /// Initialize the visualizer bound message with the visualizer and variable bound to the visualizer.
        /// </summary>
        /// <param name="theVisualizer">Visualizers being bound to a variable.</param>
        /// <param name="theVariable">Variable bound to the visualizer. 
        /// May be null if the variable is being unbound from any variable.</param>
        public VisualizerBoundMessage(VisualizerModel theVisualizer, VariableGraphicViewModel theVariable)
        {
            Contract.Requires<ArgumentNullException>(theVisualizer != null);
            Visualizer = theVisualizer;
            Variable = theVariable;
        }

        /// <summary>
        /// Gets the visualizer binding to the variable.
        /// </summary>
        public VisualizerModel Visualizer { get; private set; }

        /// <summary>
        /// Gets the variable binding to the visualizer.
        /// </summary>
        public VariableGraphicViewModel Variable { get; private set; }
    }
}