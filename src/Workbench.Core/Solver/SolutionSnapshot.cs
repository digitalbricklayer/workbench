using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// One solution to the model.
    /// </summary>
    public sealed class SolutionSnapshot
    {
        private readonly List<ValueModel> singletonValues;
        private readonly List<ValueModel> aggregateValues;

        /// <summary>
        /// Initialize a solution snapshot with variable values.
        /// </summary>
        public SolutionSnapshot(IEnumerable<ValueModel> theSingletonValues,
                                IEnumerable<ValueModel> theAggregateValues)
        {
            Contract.Requires<ArgumentNullException>(theSingletonValues != null);
            Contract.Requires<ArgumentNullException>(theAggregateValues != null);

            this.singletonValues = new List<ValueModel>(theSingletonValues);
            this.aggregateValues = new List<ValueModel>(theAggregateValues);
        }

        /// <summary>
        /// Initialize a solution snapshot with default values.
        /// </summary>
        public SolutionSnapshot()
        {
            this.singletonValues = new List<ValueModel>();
            this.aggregateValues = new List<ValueModel>();
        }

        /// <summary>
        /// Gets the singleton variable values.
        /// </summary>
        public IReadOnlyCollection<ValueModel> SingletonValues
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<ValueModel>>() != null);
                return this.singletonValues.ToList();
            }
        }

        /// <summary>
        /// Gets the aggregate variable values.
        /// </summary>
        public IReadOnlyCollection<ValueModel> AggregateValues
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<ValueModel>>() != null);
                return this.aggregateValues.ToList();
            }
        }

        /// <summary>
        /// Add a singleton variable value to the snapshot.
        /// </summary>
        /// <param name="newSingletonValue">Singleton value.</param>
        public void AddSingletonValue(ValueModel newSingletonValue)
        {
            Contract.Requires<ArgumentNullException>(newSingletonValue != null);
            this.singletonValues.Add(newSingletonValue);
        }

        /// <summary>
        /// Add an aggregate variable value to the snapshot.
        /// </summary>
        /// <param name="newAggregateValue">Aggregate value.</param>
        public void AddAggregateValue(ValueModel newAggregateValue)
        {
            Contract.Requires<ArgumentNullException>(newAggregateValue != null);
            this.aggregateValues.Add(newAggregateValue);
        }

        /// <summary>
        /// Get the aggregate variable value matching the variable name.
        /// </summary>
        /// <param name="theAggregateVariableName">Aggregate variable name.</param>
        /// <returns>Value for the aggregate variable.</returns>
        public ValueModel GetAggregateVariableValueByName(string theAggregateVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theAggregateVariableName));
            return this.aggregateValues.FirstOrDefault(_ => _.Variable.Name == theAggregateVariableName);
        }

        /// <summary>
        /// Get the singleton variable value matching the variable name.
        /// </summary>
        /// <param name="theSingletonVariableName">Singleton variable name.</param>
        /// <returns>Value for the singleton variable.</returns>
        public ValueModel GetSingletonVariableValueByName(string theSingletonVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theSingletonVariableName));
            return this.singletonValues.FirstOrDefault(_ => _.VariableName == theSingletonVariableName);
        }
    }
}