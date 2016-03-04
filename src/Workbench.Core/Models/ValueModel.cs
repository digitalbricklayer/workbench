using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Value(s) bound to a variable.
    /// </summary>
    [Serializable]
    public class ValueModel
    {
        private readonly List<int> values;

        /// <summary>
        /// Initialize the value model with the variable and 
        /// values to bind to the variables.
        /// </summary>
        /// <param name="theModel">Variable model.</param>
        /// <param name="theValues">Values to bind to the model.</param>
        public ValueModel(VariableModel theModel, IReadOnlyCollection<int> theValues)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theValues != null);
            Contract.Requires<ArgumentException>(theValues.Any());
            this.Variable = theModel;
            this.values = new List<int>(theValues);
        }

        /// <summary>
        /// Initialize the value model with the variable and 
        /// value to bind to the variables.
        /// </summary>
        /// <param name="theModel">Variable model.</param>
        /// <param name="theValue">Value to bind to the model.</param>
        public ValueModel(VariableModel theModel, int theValue)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.Variable = theModel;
            this.values = new List<int> {theValue};
        }

        /// <summary>
        /// Gets the variable associated with the values.
        /// </summary>
        public VariableModel Variable { get; private set; }

        /// <summary>
        /// Gets the values bound to the aggregate variable.
        /// </summary>
        public IReadOnlyCollection<int> Values
        {
            get
            {
                Contract.Assume(this.values != null);
                return this.values.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the first value.
        /// </summary>
        public int Value
        {
            get
            {
                Contract.Assume(this.values != null);
                return this.GetValueAt(1);
                
            }
            set
            {
                this.values[0] = value;
            }
        }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string VariableName
        {
            get
            {
                Contract.Assume(this.Variable != null);
                return this.Variable.Name;
            }
        }

        /// <summary>
        /// Get the value at the index.
        /// </summary>
        /// <param name="index">Index starting at one.</param>
        /// <returns>Value at index.</returns>
        public int GetValueAt(int index)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index <= this.Values.Count);
            return this.values[index - 1];
        }
    }
}