using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Cache to track model elements to solver equivalents.
    /// </summary>
    internal class OrangeCache
    {
        private readonly Dictionary<string, OrangeSingletonVariableMap> _singletonVariableMap;
        private readonly Dictionary<string, OrangeAggregateVariableMap> _aggregateVariableMap;

        /// <summary>
        /// Initialize a cache with default values.
        /// </summary>
        internal OrangeCache()
        {
            _singletonVariableMap = new Dictionary<string, OrangeSingletonVariableMap>();
            _aggregateVariableMap = new Dictionary<string, OrangeAggregateVariableMap>();
        }

        internal void AddSingleton(string name, OrangeSingletonVariableMap singletonVariableMap)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            _singletonVariableMap.Add(name, singletonVariableMap);
        }

        internal void AddAggregate(string name, OrangeAggregateVariableMap aggregateVariableMap)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            _aggregateVariableMap.Add(name, aggregateVariableMap);
        }

        internal IntegerVariable GetSolverSingletonVariableByName(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            return _singletonVariableMap[variableName].SolverVariable;
        }

        internal SingletonVariableModel GetModelSingletonVariableByName(string variableName)
        {
            if (!_singletonVariableMap.ContainsKey(variableName))
                throw new ArgumentException($"Unknown variable {variableName}");
            return _singletonVariableMap[variableName].ModelVariable;
        }

        internal IntegerVariable GetSolverAggregateVariableByName(string variableName, int index)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            return _aggregateVariableMap[variableName].GetAt(index);
        }

        internal AggregateIntegerVariable GetSolverAggregateVariableByName(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            return _aggregateVariableMap[variableName].SolverVariable;
        }

        internal AggregateVariableModel GetModelAggregateVariableByName(string variableName)
        {
            if (!_aggregateVariableMap.ContainsKey(variableName))
                throw new ArgumentException($"Unknown variable {variableName}");
            return _aggregateVariableMap[variableName].ModelVariable;
        }
    }
}