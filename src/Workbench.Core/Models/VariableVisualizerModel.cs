using System;
using System.Windows;

namespace Workbench.Core.Models
{
    [Serializable]
    public class VariableVisualizerModel : GraphicModel
    {
        private VariableModel boundVariable;

        public VariableVisualizerModel(Point newLocation)
            : base("A visualizer", newLocation)
        {
        }

        /// <summary>
        /// Gets the variable the visualizer is bound.
        /// </summary>
        public VariableModel BoundTo
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
        /// Bind the visualizer to a variable.
        /// </summary>
        /// <param name="theVariable">Variable to bind.</param>
        public void BindTo(VariableModel theVariable)
        {
            if (theVariable == null)
                throw new ArgumentNullException("theVariable");
            this.BoundTo = theVariable;
        }
    }
}
