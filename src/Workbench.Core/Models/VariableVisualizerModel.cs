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
        /// Gets the bound variable.
        /// </summary>
        public VariableModel BoundVariable
        {
            get
            {
                return this.boundVariable;
            }
            private set
            {
                this.boundVariable = value;
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
            this.BoundVariable = theVariable;
        }
    }
}
