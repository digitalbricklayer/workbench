using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An aggregate variable label matches the values bound to an aggregate variable with the variable itself.
    /// </summary>
    [Serializable]
    public class AggregateVariableLabelModel : LabelModel
    {
        private readonly List<ValueModel> values;

        /// <summary>
        /// Initialize an aggregate label with the variable and values.
        /// </summary>
        /// <param name="theAggregateVariable">Variable model.</param>
        /// <param name="theValues">Values to bind to the model.</param>
        public AggregateVariableLabelModel(AggregateVariableModel theAggregateVariable, IReadOnlyCollection<ValueModel> theValues)
            : base(theAggregateVariable)
        {
            if (!theValues.Any())
                throw new ArgumentException(nameof(theValues));

            AggregateVariable = theAggregateVariable;
            this.values = new List<ValueModel>(theValues);
        }

        /// <summary>
        /// Gets the variable associated with the values.
        /// </summary>
        public AggregateVariableModel AggregateVariable { get; private set; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        public IReadOnlyCollection<ValueModel> Bindings
        {
            get
            {
                Debug.Assert(this.values != null);
                return new ReadOnlyCollection<ValueModel>(this.values);
            }
        }

        /// <summary>
        /// Gets the model values.
        /// </summary>
        public IReadOnlyCollection<object> Values
        {
            get
            {
                Debug.Assert(this.values != null);
                var theValues = this.values.Select(binding => binding.Model)
                                           .ToList();
                return new ReadOnlyCollection<object>(theValues);
            }
        }

        /// <summary>
        /// Gets a text representation of the value.
        /// </summary>
        public override string Text
        {
            get
            {
                var textBuilder = new StringBuilder();
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
            if (index <= 0 && index >= Values.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var theValue = this.values[index];
            return theValue.Model;
        }

        /// <summary>
        /// Get the value at the index.
        /// </summary>
        /// <param name="index">Index starting at zero.</param>
        /// <returns>Value at index.</returns>
        public ValueModel GetBindingAt(int index)
        {
            if (index <= 0 && index >= Values.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.values[index];
        }
    }
}
