using System;
using System.Windows;

namespace Workbench.Core.Models
{
    [Serializable]
    public class VisualizerModel : GraphicModel
    {
        private VariableVisualizerBindingModel boundVariable;
        private ValueModel value;

        protected VisualizerModel(string graphicName, Point location, VariableModel theBoundVariable) : base(graphicName, location)
        {
            this.Binding = new VariableVisualizerBindingModel(this, theBoundVariable);
        }

        protected VisualizerModel(string graphicName, Point location) : base(graphicName, location)
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