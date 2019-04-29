using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Extracts possible values for variables from constraint expressions.
    /// </summary>
    internal sealed class PossibleValueExtractor
    {
        private readonly OrangeModelSolverMap _modelSolverMap;

        /// <summary>
        /// Initialize a possible value extractor with a model solver map.
        /// </summary>
        /// <param name="modelSolverMap">Model to solver map.</param>
        internal PossibleValueExtractor(OrangeModelSolverMap modelSolverMap)
        {
            Contract.Requires<ArgumentNullException>(modelSolverMap != null);
            _modelSolverMap = modelSolverMap;
        }

        /// <summary>
        /// Extract possible values from the node.
        /// </summary>
        /// <param name="expression">Node from an arc.</param>
        /// <returns>Possible values for the node.</returns>
        internal DomainRange ExtractFrom(ExpressionNode expression)
        {
            switch (expression.InnerExpression)
            {
                case SingletonVariableReferenceExpressionNode singletonVariableReferenceExpressionNode:
                    var x = _modelSolverMap.GetSolverSingletonVariableByName(singletonVariableReferenceExpressionNode.VariableReference.VariableName);
                    return x.Domain;

                case AggregateVariableReferenceExpressionNode aggregateVariableReferenceExpressionNode:
                    return GetRangeFrom(aggregateVariableReferenceExpressionNode);

                case SingletonVariableReferenceNode singletonVariableReferenceNode:
                    var singletonSolverVariable = _modelSolverMap.GetSolverSingletonVariableByName(singletonVariableReferenceNode.VariableName);
                    return singletonSolverVariable.Domain;

                case AggregateVariableReferenceNode aggregateVariableReferenceNode:
                    return GetRangeFrom(aggregateVariableReferenceNode);

                default:
                    throw new NotImplementedException();
            }
        }

        private DomainRange GetRangeFrom(AggregateVariableReferenceNode aggregateVariableReferenceNode)
        {
            var aggregateSolverVariable = _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariableReferenceNode.VariableName);
            var subscriptVariable = aggregateSolverVariable.GetAt(aggregateVariableReferenceNode.SubscriptStatement.Subscript);
            return subscriptVariable.Domain;
        }

        private DomainRange GetRangeFrom(AggregateVariableReferenceExpressionNode aggregateVariableReferenceExpressionNode)
        {
            var subscriptVariable = _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariableReferenceExpressionNode.VariableReference.VariableName,
                                                                                     aggregateVariableReferenceExpressionNode.VariableReference.SubscriptStatement.Subscript);
            return subscriptVariable.Domain;
        }
    }
}