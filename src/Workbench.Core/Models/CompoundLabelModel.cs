using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A compound label matches the values bound to an aggregate variable with the variable itself.
    /// </summary>
    [Serializable]
    public class CompoundLabelModel
    {
        private readonly List<ValueBinding> valueBindings;

        /// <summary>
        /// Initialize a compound label with the variable and valueBindings.
        /// </summary>
        /// <param name="theModel">Variable model.</param>
        /// <param name="theValueBindings">Values to bind to the model.</param>
        public CompoundLabelModel(VariableModel theModel, IReadOnlyCollection<ValueBinding> theValueBindings)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theValueBindings != null);
            Contract.Requires<ArgumentException>(theValueBindings.Any());

            Variable = theModel;
            this.valueBindings = new List<ValueBinding>(theValueBindings);
        }

        /// <summary>
        /// Gets the variable associated with the valueBindings.
        /// </summary>
        public VariableModel Variable { get; private set; }

        /// <summary>
        /// Gets the value bindings.
        /// </summary>
        public IReadOnlyCollection<ValueBinding> Bindings
        {
            get
            {
                Contract.Assume(this.valueBindings != null);
                return new ReadOnlyCollection<ValueBinding>(this.valueBindings);
            }
        }

        /// <summary>
        /// Gets the model values.
        /// </summary>
        public IReadOnlyCollection<object> Values
        {
            get
            {
                Contract.Assume(this.valueBindings != null);
                var theValues = this.valueBindings.Select(binding => binding.Model)
                                           .ToList();
                return new ReadOnlyCollection<object>(theValues);
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

            var theValue = this.valueBindings[index];
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

            return this.valueBindings[index];
        }
    }
}