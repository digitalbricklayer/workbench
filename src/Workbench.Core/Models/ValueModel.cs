using System;
using System.Collections.Generic;

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
        public ValueModel(VariableModel theModel, IEnumerable<int> theValues)
        {
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
            return this.values[index - 1];
        }
    }
}