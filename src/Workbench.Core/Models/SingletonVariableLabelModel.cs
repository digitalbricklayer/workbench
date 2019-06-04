using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A label matching a value bound to a singleton variable.
    /// </summary>
    [Serializable]
    public class SingletonVariableLabelModel : LabelModel
    {
        private readonly ValueModel value;

        /// <summary>
        /// Initialize the label with the variable and binding.
        /// </summary>
        /// <param name="theVariable">Variable model.</param>
        /// <param name="theValue">Value to bind to the model.</param>
        public SingletonVariableLabelModel(SingletonVariableModel theVariable, ValueModel theValue)
            : base(theVariable)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            Contract.Requires<ArgumentNullException>(theValue != null);

            SingletonVariable = theVariable;
            this.value = theValue;
        }

        public SingletonVariableModel SingletonVariable { get; }

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

        public override string Text
        {
            get { return Value.ToString(); }
        }

        public override string ToString()
        {
            return $"<{VariableName},{Value}>";
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
