using System;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A variable graphic.
    /// </summary>
    [Serializable]
    public abstract class VariableGraphicModel : GraphicModel
    {
        private VariableModel variable;

        /// <summary>
        /// Initializes a variable with default values.
        /// </summary>
        protected VariableGraphicModel(VariableModel theVariable, Point theLocation)
            : base(theVariable, theLocation)
        {
            Variable = theVariable;
        }

        /// <summary>
        /// Gets or sets the variable domain expression.
        /// </summary>
        public VariableDomainExpressionModel DomainExpression
        {
            get { return Variable.DomainExpression; }
            set
            {
                Variable.DomainExpression = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the variable model.
        /// </summary>
        public VariableModel Variable
        {
            get
            {
                return this.variable;
            }
            set
            {
                this.variable = value;
            }
        }

        /// <summary>
        /// Returns a string that represents the variable.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Variable.Name.Text;
        }

        /// <summary>
        /// Get the size of the variable.
        /// </summary>
        /// <returns>Size of the variable.</returns>
        public virtual long GetSize()
        {
            return Variable.GetSize();
        }
    }
}
