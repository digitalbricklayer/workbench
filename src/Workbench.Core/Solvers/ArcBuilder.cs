using System;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class ArcBuilder
    {
        private readonly OrangeModelSolverMap _modelSolverMap;

        /// <summary>
        /// Initialize an arc builder with a model solver map.
        /// </summary>
        /// <param name="modelSolverMap">Map between solver and model entities.</param>
        internal ArcBuilder(OrangeModelSolverMap modelSolverMap)
        {
            _modelSolverMap = modelSolverMap;
        }

        /// <summary>
        /// Build an arc from an expression constraint.
        /// </summary>
        /// <param name="expressionConstraintNode">Expression constraint node.</param>
        /// <returns>Arc</returns>
        internal Arc Build(ConstraintExpressionNode expressionConstraintNode)
        {
            var left = CreateNodeFrom(expressionConstraintNode.InnerExpression.LeftExpression);
            if (!expressionConstraintNode.InnerExpression.RightExpression.IsLiteral)
            {
                var right = CreateNodeFrom(expressionConstraintNode.InnerExpression.RightExpression);
                return new Arc(left, right, CreateConnectorFrom(left, right, CreateConstraintFrom(expressionConstraintNode)));
            }

            return new Arc(left, left, CreateConnectorFrom(left, left, CreateConstraintFrom(expressionConstraintNode)));
        }

        private Node CreateNodeFrom(ExpressionNode expressionConstraintNode)
        {
            var variable = ExtractVariableFrom(expressionConstraintNode);
            var existingNode = _modelSolverMap.GetNodeByName(variable.Name);

            return existingNode ?? new Node(variable);
        }

        private IntegerVariable ExtractVariableFrom(ExpressionNode expressionConstraintNode)
        {
            switch (expressionConstraintNode.InnerExpression)
            {
                case SingletonVariableReferenceNode singletonVariableReference:
                    return _modelSolverMap.GetSolverSingletonVariableByName(singletonVariableReference.VariableName);

                case AggregateVariableReferenceNode aggregateVariableReference:
                    return _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariableReference.VariableName,
                                                                            aggregateVariableReference.SubscriptStatement.Subscript);

                case SingletonVariableReferenceExpressionNode singletonVariableExpression:
                    return _modelSolverMap.GetSolverSingletonVariableByName(singletonVariableExpression.VariableReference.VariableName);

                case AggregateVariableReferenceExpressionNode aggregateVariableExpression:
                    return _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariableExpression.VariableReference.VariableName,
                                                                            aggregateVariableExpression.VariableReference.SubscriptStatement.Subscript);

                default:
                    throw new NotImplementedException();
            }
        }

        private ConstraintExpression CreateConstraintFrom(ConstraintExpressionNode binaryExpressionNode)
        {
            return new ConstraintExpression(binaryExpressionNode);
        }

        private NodeConnector CreateConnectorFrom(Node left, Node right, ConstraintExpression constraint)
        {
            return new NodeConnector(left, right, constraint);
        }
    }
}
