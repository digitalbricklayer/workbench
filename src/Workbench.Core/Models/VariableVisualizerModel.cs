using System;
using System.Diagnostics.Contracts;
using System.Windows;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Model for the variable visualizer.
    /// </summary>
    [Serializable]
    public class VariableVisualizerModel : VisualizerModel
    {
        private VariableVisualizerBindingModel binding;
        private ValueModel value;

        /// <summary>
        /// Initialize the variable visualizer with the location and bound variable.
        /// </summary>
        /// <param name="newLocation">The graphic location.</param>
        /// <param name="theBoundVariable">The variable bound to the visualizer.</param>
        public VariableVisualizerModel(Point newLocation, VariableModel theBoundVariable)
            : base(theBoundVariable.Name, newLocation)
        {
            Contract.Requires<ArgumentNullException>(theBoundVariable != null);
            Binding = new VariableVisualizerBindingModel(this);
            Binding.BindTo(theBoundVariable);
        }

        /// <summary>
        /// Initialize the variable visualizer with the location.
        /// </summary>
        /// <param name="newLocation">The graphic location.</param>
        public VariableVisualizerModel(Point newLocation)
            : base("A visualizer", newLocation)
        {
            Binding = new VariableVisualizerBindingModel(this);
        }

        /// <summary>
        /// Gets the visualizer binding expression.
        /// </summary>
        public VariableVisualizerBindingModel Binding
        {
            get
            {
                return this.binding;
            }
            private set
            {
                this.binding = value;
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
        /// Bind the visualizer to a variable from the snapshot.
        /// </summary>
        /// <param name="theVariable">Variable to bind the variable visualizer.</param>
        public void BindTo(VariableModel theVariable)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            Binding.BindTo(theVariable);
        }

        /// <summary>
        /// Bind the visualizer to a variable from the snapshot.
        /// </summary>
        /// <param name="theContext">Context for updating a visualizer.</param>
        public override void UpdateFrom(VisualizerUpdateContext theContext)
        {
        }

        /// <summary>
        /// Update a visualizer with call arguments.
        /// </summary>
        /// <param name="theCall">Call arguments.</param>
        public override void UpdateWith(VisualizerCall theCall)
        {
            throw new NotImplementedException();
        }
    }
}
