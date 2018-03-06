using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A label matches the values bound to a singleton variable with the variable itself.
    /// </summary>
    [Serializable]
    public class LabelModel
    {
        private readonly ValueBinding valueBinding;

        /// <summary>
        /// Initialize the label with the variable and binding.
        /// </summary>
        /// <param name="theVariable">Variable model.</param>
        /// <param name="theBinding">Value to bind to the model.</param>
        public LabelModel(VariableGraphicModel theVariable, ValueBinding theBinding)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            Contract.Requires<ArgumentNullException>(theBinding != null);

            Variable = theVariable;
            this.valueBinding = theBinding;
        }

        /// <summary>
        /// Gets the variable associated with the values.
        /// </summary>
        public VariableGraphicModel Variable { get; private set; }

        /// <summary>
        /// Gets the bindings bound to the variable.
        /// </summary>
        public ValueBinding Binding
        {
            get
            {
                return this.valueBinding;
            }
        }

        /// <summary>
        /// Gets or sets the first binding.
        /// </summary>
        public object Value
        {
            get
            {
                Contract.Assume(this.valueBinding != null);
                return this.valueBinding.Model;
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
                return Variable.Name;
            }
        }

        /// <summary>
        /// Gets a text representation of the binding.
        /// </summary>
        public string Text
        {
            get
            {
                return valueBinding.ToString();
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
