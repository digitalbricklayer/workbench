using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Cache to map model elements to solver equivalents for the Orange solver.
    /// </summary>
    internal class OrangeModelSolverMap
    {
        private readonly Dictionary<string, OrangeSingletonVariableMap> _singletonVariableMap;
        private readonly Dictionary<string, OrangeAggregateVariableMap> _aggregateVariableMap;
        private readonly Dictionary<string, Node> _nodeMap;
        private readonly List<TernaryConstraintExpressionSolution> _ternarySolutions;

        /// <summary>
        /// Initialize a cache with default values.
        /// </summary>
        internal OrangeModelSolverMap()
        {
            _singletonVariableMap = new Dictionary<string, OrangeSingletonVariableMap>();
            _aggregateVariableMap = new Dictionary<string, OrangeAggregateVariableMap>();
            _nodeMap = new Dictionary<string, Node>();
            _ternarySolutions = new List<TernaryConstraintExpressionSolution>();
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

        /// <summary>
        /// Add a node to the map.
        /// </summary>
        /// <param name="name">Name of the variable inside the node.</param>
        /// <param name="node">Node.</param>
        internal void AddNode(string name, Node node)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            _nodeMap.Add(name, node);
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

        internal AggregateVariableModel GetModelAggregateVariableByName(string variableName)
        {
            if (!_aggregateVariableMap.ContainsKey(variableName))
                throw new ArgumentException($"Unknown variable {variableName}");
            return _aggregateVariableMap[variableName].ModelVariable;
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

        internal IEnumerable<TernaryConstraintExpressionSolution> FindTernaryConstraintSolutionsInvolving(SolverVariable singletonVariable)
        {
            var solutionAccumulator = new List<TernaryConstraintExpressionSolution>();
            foreach (var ternaryConstraintSolution in _ternarySolutions)
            {
                var leftVariable = ExtractVariableFrom(ternaryConstraintSolution.Expression.ExpressionNode.InnerExpression.LeftExpression);

                if (leftVariable.Name == singletonVariable.Name)
                {
                    solutionAccumulator.Add(ternaryConstraintSolution);
                }
                else
                {
                    if (!ternaryConstraintSolution.Expression.ExpressionNode.InnerExpression.RightExpression.IsLiteral)
                    {
                        var rightVariable = ExtractVariableFrom(ternaryConstraintSolution.Expression.ExpressionNode.InnerExpression.RightExpression);
                        if (rightVariable.Name == singletonVariable.Name)
                        {
                            solutionAccumulator.Add(ternaryConstraintSolution);
                        }
                    }
                }
            }

            return solutionAccumulator;
        }

        // Copied from ArcBuilder.
        private SolverVariable ExtractVariableFrom(ExpressionNode expressionConstraintNode)
        {
            switch (expressionConstraintNode.InnerExpression)
            {
                case SingletonVariableReferenceNode singletonVariableReference:
                    return GetSolverSingletonVariableByName(singletonVariableReference.VariableName);

                case AggregateVariableReferenceNode aggregateVariableReference:
                    return GetSolverAggregateVariableByName(aggregateVariableReference.VariableName,
                                                            aggregateVariableReference.SubscriptStatement.Subscript);

                case SingletonVariableReferenceExpressionNode singletonVariableExpression:
                    return GetSolverSingletonVariableByName(singletonVariableExpression.VariableReference.VariableName);

                case AggregateVariableReferenceExpressionNode aggregateVariableExpression:
                    return GetSolverAggregateVariableByName(aggregateVariableExpression.VariableReference.VariableName,
                                                            aggregateVariableExpression.VariableReference.SubscriptStatement.Subscript);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get all ternary constraint expression solutions.
        /// </summary>
        /// <returns>All ternary constraint expression solutions.</returns>
        internal IReadOnlyCollection<TernaryConstraintExpressionSolution> GetTernaryConstraintExpressionSolutions()
        {
            return _ternarySolutions.AsReadOnly();
        }
    }
}
