using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class VisualizerModel : GraphicModel
    {
        private VariableVisualizerBindingModel boundVariable;
        private ValueModel value;

        /// <summary>
        /// Initialize a bound visualizer with a name, location and bound variable.
        /// </summary>
        /// <param name="graphicName">Visualizer name.</param>
        /// <param name="location">Location.</param>
        /// <param name="theBoundVariable">Bound variable.</param>
        protected VisualizerModel(string graphicName,
                                  Point location,
                                  VariableModel theBoundVariable)
            : base(graphicName, location)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(graphicName));
            Contract.Requires<ArgumentNullException>(theBoundVariable != null);
            Binding = new VariableVisualizerBindingModel(this, theBoundVariable);
        }

        /// <summary>
        /// Initialize an unbound visualizer with a name and location.
        /// </summary>
        /// <param name="graphicName">Visualizer name.</param>
        /// <param name="location">Location.</param>
        protected VisualizerModel(string graphicName, Point location)
            : base(graphicName, location)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(graphicName));
            Binding = new VariableVisualizerBindingModel(this);
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
        public virtual void BindTo(VariableModel theVariable)
        {
            Binding.BindTo(theVariable);
        }

        /// <summary>
        /// Hydrate the visualizer from the value.
        /// </summary>
        /// <param name="theValue">Value bound to a variable.</param>
        public virtual void Hydrate(ValueModel theValue)
        {
            Contract.Requires<ArgumentNullException>(theValue != null);
            Value = theValue;
        }
    }
}
