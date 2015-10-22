using System;
using System.Collections.Generic;
using System.Linq;

namespace Dyna.Core.Models
{
    /// <summary>
    /// An aggregate variable can hold zero or more variables.
    /// </summary>
    [Serializable]
    public class AggregateVariableModel : GraphicModel
    {
        private VariableModel[] variables;

        /// <summary>
        /// Initialize an aggregate variable with default values.
        /// </summary>
        public AggregateVariableModel()
        {
            this.variables = new VariableModel[0];
        }

        /// <summary>
        /// Initialize an aggregate variable with a name.
        /// </summary>
        /// <param name="newName">New variable name.</param>
        public AggregateVariableModel(string newName)
            : base(newName)
        {
            this.variables = new VariableModel[0];
        }

        /// <summary>
        /// Gets the variables in the aggregate.
        /// </summary>
        public IEnumerable<VariableModel> Variables
        {
            get
            {
                return this.variables.AsEnumerable();
            }
        }

        /// <summary>
        /// Gets a count of the variables in the aggregate.
        /// </summary>
        public int AggregateCount
        {
            get
            {
                return this.variables.Length;
            }
        }

        /// <summary>
        /// Resize the aggregate variable.
        /// </summary>
        /// <param name="newAggregateSize">New aggregate size.</param>
        public void Resize(int newAggregateSize)
        {
            if (this.variables.Length == newAggregateSize) return;
            var originalAggregateSize = this.variables.Length;
            Array.Resize(ref this.variables, newAggregateSize);
            var newAggregateCount = originalAggregateSize > newAggregateSize ? newAggregateSize : originalAggregateSize;
            // Fill the new array elements with a default variable model
            for (var i = newAggregateCount; i < newAggregateSize; i++)
                this.variables[i] = new VariableModel();
        }
    }
}
