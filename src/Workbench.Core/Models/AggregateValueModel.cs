using System;
using System.Collections.Generic;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Value(s) bound to an aggregate variable.
    /// </summary>
    [Serializable]
    public class AggregateValueModel : GraphicModel
    {
        private readonly List<int> values;

        /// <summary>
        /// Initialize the aggregate value model with the aggregate variable and 
        /// values to bind to the variables.
        /// </summary>
        /// <param name="theModel">Aggregate variable model.</param>
        /// <param name="theValues">Values to bind to the model.</param>
        public AggregateValueModel(AggregateVariableModel theModel, IEnumerable<int> theValues)
        {
            this.Variable = theModel;
            this.values = new List<int>(theValues);
        }

        /// <summary>
        /// Initialize an aggregate value with default values.
        /// </summary>
        public AggregateValueModel()
        {
            this.values = new List<int>();
        }

        /// <summary>
        /// Gets the variable associated with the values.
        /// </summary>
        public AggregateVariableModel Variable { get; private set; }

        /// <summary>
        /// Gets the values bound to the aggregate variable.
        /// </summary>
        public IEnumerable<int> Values
        {
            get
            {
                return this.values;
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