using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Value(s) bound to a variable.
    /// </summary>
    [Serializable]
    public class ValueModel
    {
        private readonly List<ValueBinding> values;

        /// <summary>
        /// Initialize the value model with the variable and 
        /// values to bind to the variables.
        /// </summary>
        /// <param name="theModel">Variable model.</param>
        /// <param name="theValues">Values to bind to the model.</param>
        public ValueModel(VariableGraphicModel theModel, IReadOnlyCollection<ValueBinding> theValues)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theValues != null);
            Contract.Requires<ArgumentException>(theValues.Any());
            Variable = theModel;
            this.values = new List<ValueBinding>(theValues);
        }

        /// <summary>
        /// Return the value as an integer.
        /// </summary>
        /// <returns>Value as an integer.</returns>
        public int GetValueAsInt()
        {
            return Convert.ToInt32(Value);
        }

        /// <summary>
        /// Initialize the value model with the variable and 
        /// value to bind to the variables.
        /// </summary>
        /// <param name="theModel">Variable model.</param>
        /// <param name="theValue">Value to bind to the model.</param>
        public ValueModel(VariableGraphicModel theModel, ValueBinding theValue)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Variable = theModel;
            this.values = new List<ValueBinding> {theValue};
        }

        /// <summary>
        /// Gets the variable associated with the values.
        /// </summary>
        public VariableGraphicModel Variable { get; private set; }

        /// <summary>
        /// Gets the bindings bound to the variable.
        /// </summary>
        public IReadOnlyCollection<ValueBinding> Bindings
        {
            get
            {
                Contract.Assume(this.values != null);
                return new ReadOnlyCollection<ValueBinding>(this.values);
            }
        }

        /// <summary>
        /// Gets the model values bound to the variable.
        /// </summary>
        public IReadOnlyCollection<object> Values
        {
            get
            {
                Contract.Assume(this.values != null);
                var theValues = new List<object>();
                foreach (var valueBinding in this.values)
                {
                    theValues.Add(valueBinding.Model);
                }
                return new ReadOnlyCollection<object>(theValues);
            }
        }

        /// <summary>
        /// Gets or sets the first value.
        /// </summary>
        public object Value
        {
            get
            {
                Contract.Assume(this.values != null);
                return GetValueAt(0);
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
        /// Gets a text representation of the value.
        /// </summary>
        public string Text
        {
            get
            {
                var textBuilder = new StringBuilder();
                textBuilder.Append(VariableName);
                foreach (var value in Values)
                {
                    textBuilder.Append(" " + value);
                }

                return textBuilder.ToString();
            }
        }

        /// <summary>
        /// Get the value at the index.
        /// </summary>
        /// <param name="index">Index starting at zero.</param>
        /// <returns>Value at index.</returns>
        public object GetValueAt(int index)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < Values.Count);

            var theValue = this.values[index];
            return theValue.Model;
        }

        /// <summary>
        /// Get the value at the index.
        /// </summary>
        /// <param name="index">Index starting at zero.</param>
        /// <returns>Value at index.</returns>
        public ValueBinding GetBindingAt(int index)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < Values.Count);

            return this.values[index];
        }
    }
}
