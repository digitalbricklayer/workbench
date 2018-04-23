using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A label matches a value bound to a singleton variable with the variable.
    /// </summary>
    [Serializable]
    public class LabelModel
    {
        private readonly ValueModel value;

        /// <summary>
        /// Initialize the label with the variable and binding.
        /// </summary>
        /// <param name="theVariable">Variable model.</param>
        /// <param name="theValue">Value to bind to the model.</param>
        public LabelModel(VariableModel theVariable, ValueModel theValue)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            Contract.Requires<ArgumentNullException>(theValue != null);

            Variable = theVariable;
            this.value = theValue;
        }

        /// <summary>
        /// Gets the variable associated with the values.
        /// </summary>
        public VariableModel Variable { get; private set; }

        /// <summary>
        /// Gets or sets the first binding.
        /// </summary>
        public object Value
        {
            get
            {
                Contract.Assume(this.value != null);
                return this.value.Model;
            }
        }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string VariableName
        {
            get
            {
                Contract.Assume(Variable != null);
                return Variable.Name.Text;
            }
        }

        /// <summary>
        /// Gets a text representation of the binding.
        /// </summary>
        public string Text
        {
            get
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Return the binding as an integer.
        /// </summary>
        /// <returns>Value as an integer.</returns>
        public int GetValueAsInt()
        {
            return Convert.ToInt32(Value);
        }
    }
}
