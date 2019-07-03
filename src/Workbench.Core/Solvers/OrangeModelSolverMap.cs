using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Cache to map model elements to solver equivalents for the Orange solver.
    /// </summary>
    internal class OrangeModelSolverMap
    {
        private readonly Dictionary<string, OrangeSingletonVariableMap> _singletonVariableMap;
        private readonly Dictionary<string, OrangeAggregateVariableMap> _aggregateVariableMap;
        private readonly Dictionary<string, SolverVariable> _solverVariableMap;
        private readonly Dictionary<string, VariableModel> _modelVariableMap;
        private readonly Dictionary<string, Node> _nodeMap;
        private readonly List<TernaryConstraintExpressionSolution> _ternarySolutions;
        private readonly Dictionary<string, OrangeBucketVariableMap> _bucketMap;

        /// <summary>
        /// Initialize a cache with default values.
        /// </summary>
        internal OrangeModelSolverMap()
        {
            _singletonVariableMap = new Dictionary<string, OrangeSingletonVariableMap>();
            _aggregateVariableMap = new Dictionary<string, OrangeAggregateVariableMap>();
            _bucketMap = new Dictionary<string, OrangeBucketVariableMap>();
            _solverVariableMap = new Dictionary<string, SolverVariable>();
            _modelVariableMap = new Dictionary<string, VariableModel>();
            _nodeMap = new Dictionary<string, Node>();
            _ternarySolutions = new List<TernaryConstraintExpressionSolution>();
        }

        internal void AddSingleton(string name, OrangeSingletonVariableMap singletonVariableMap)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            _singletonVariableMap.Add(name, singletonVariableMap);
            _solverVariableMap.Add(name, singletonVariableMap.SolverVariable);
        }

        internal void AddAggregate(string name, OrangeAggregateVariableMap aggregateVariableMap)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            _aggregateVariableMap.Add(name, aggregateVariableMap);
            foreach (var solverVariable in aggregateVariableMap.SolverVariable.Variables)
            {
                _solverVariableMap.Add(solverVariable.Name, solverVariable);
            }

            foreach (var modelVariable in aggregateVariableMap.ModelVariable.Variables)
            {
                _modelVariableMap.Add(modelVariable.Name, modelVariable);
            }
        }

        internal void AddBucket(string name, OrangeBucketVariableMap bucketVariableMap)
        {
            _bucketMap.Add(name, bucketVariableMap);
            foreach (var bundleMap in bucketVariableMap.GetBundleMaps())
            {
                foreach (var x in bundleMap.GetVariableMaps())
                {
                    _solverVariableMap.Add(x.SolverVariable.Name, x.SolverVariable);
                }
            }
        }

        internal SolverVariable GetSolverSingletonVariableByName(string variableName)
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

        internal SolverVariable GetSolverAggregateVariableByName(string variableName, int index)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            return _aggregateVariableMap[variableName].GetAt(index);
        }

        internal AggregateSolverVariable GetSolverAggregateVariableByName(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            return _aggregateVariableMap[variableName].SolverVariable;
        }

        internal SolverVariable GetSolverBucketVariableByName(string bucketName, int index, string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(bucketName));
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));

            var bucketVariableMap = _bucketMap[bucketName];
            var bundleMap = bucketVariableMap.GetBundleVariableAt(index);
            var variableMap = bundleMap.GetVariableByName(variableName);
            return variableMap.SolverVariable;
        }

        internal OrangeBucketVariableMap GetBucketVariableMapByName(string bucketName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(bucketName));

            return _bucketMap[bucketName];
        }

        internal AggregateVariableModel GetModelAggregateVariableByName(string variableName)
        {
            if (!_aggregateVariableMap.ContainsKey(variableName))
                throw new ArgumentException($"Unknown variable {variableName}");
            return _aggregateVariableMap[variableName].ModelVariable;
        }

        internal VariableModel GetInternalModelAggregateVariableByName(string variableName)
        {
            if (!_modelVariableMap.ContainsKey(variableName))
                throw new ArgumentException($"Unknown variable {variableName}");
            return _modelVariableMap[variableName];
        }

        internal Node GetNodeByName(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));

            if (_nodeMap.ContainsKey(variableName))
            {
                return _nodeMap[variableName];
            }

            return null;
        }

        internal void AddTernaryExpressionSolution(TernaryConstraintExpressionSolution solution)
        {
            _ternarySolutions.Add(solution);
        }

        /// <summary>
        /// Get all ternary constraint expression solutions.
        /// </summary>
        /// <returns>All ternary constraint expression solutions.</returns>
        internal IReadOnlyCollection<TernaryConstraintExpressionSolution> GetTernaryConstraintExpressionSolutions()
        {
            return _ternarySolutions.AsReadOnly();
        }

        internal SolverVariable GetSolverVariableByName(string variableName)
        {
            return _solverVariableMap[variableName];
        }

        internal IEnumerable<OrangeBucketVariableMap> GetBucketVariables()
        {
            return _bucketMap.Values;
        }
    }
}
