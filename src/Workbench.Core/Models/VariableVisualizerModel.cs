using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Model for the variable visualizer.
    /// </summary>
    [Serializable]
    public class VariableVisualizerModel : GraphicModel
    {
        private VariableVisualizerBindingModel boundVariable;
        private ValueModel value;

        /// <summary>
        /// Initialize the variable visualizer with the location and variable binding.
        /// </summary>
        /// <param name="newLocation">The graphic location.</param>
        /// <param name="theBinding">The variable binding.</param>
        public VariableVisualizerModel(Point newLocation, VariableModel theBinding)
            : base("A visualizer", newLocation)
        {
            Contract.Requires<ArgumentNullException>(theBinding != null);
            this.Binding = new VariableVisualizerBindingModel(this, theBinding);
        }

        /// <summary>
        /// Initialize the variable visualizer with the location.
        /// </summary>
        /// <param name="newLocation">The graphic location.</param>
        public VariableVisualizerModel(Point newLocation)
            : base("A visualizer", newLocation)
        {
            this.Binding = new VariableVisualizerBindingModel(this);
        }

        /// <summary>
        /// Gets the visualizer binding.
        /// </summary>
        public VariableVisualizerBindingModel Binding
        {
            get
            {
                return this.boundVariable;
            }
            private set
            {
                this.boundVariable = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the variable value.
        /// </summary>
        public ValueModel Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Bind the visualizer to a variable.
        /// </summary>
        /// <param name="theVariable">Variable to bind.</param>
        /// <remarks>
        /// The variable may be null when binding to a non-existent variable.
        /// </remarks>
        public void BindTo(VariableModel theVariable)
        {
            this.Binding.BindTo(theVariable);
        }
    }
}
