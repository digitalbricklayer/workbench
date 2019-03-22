using System;
using Workbench.Core.Models;
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

        internal Arc Build(ExpressionConstraintModel expressionConstraint)
        {
            return new Arc(CreateNodeFrom(expressionConstraint.Expression.Node.InnerExpression.LeftExpression),
                           CreateNodeFrom(expressionConstraint.Expression.Node.InnerExpression.RightExpression), 
                           CreateConnectorFrom(CreateConstraintFrom(expressionConstraint.Expression.Node)));
        }

        private ConstraintExpression CreateConstraintFrom(ConstraintExpressionNode binaryExpressionNode)
        {
            return new ConstraintExpression(binaryExpressionNode);
        }

        private NodeConnector CreateConnectorFrom(ConstraintExpression constraint)
        {
            return new NodeConnector(constraint);
        }

        private Node CreateNodeFrom(ExpressionNode expressionNode)
        {
            switch (expressionNode.InnerExpression)
            {
                case SingletonVariableReferenceNode singletonVariableReference:
                    return new VariableNode(_modelSolverMap.GetSolverSingletonVariableByName(singletonVariableReference.VariableName));

                case AggregateVariableReferenceNode aggregateVariableReference:
                    var variable = _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariableReference.VariableName,
                                                                                    aggregateVariableReference.SubscriptStatement.Subscript);
                    return new VariableNode(variable);

                case IntegerLiteralNode literalNode:
                    return new LiteralNode(literalNode.Value);

                default:
                    throw new NotImplementedException();
            }
        }

        private Node CreateNodeFrom(IntegerVariable variable)
        {
            return new VariableNode(variable);
        }
    }
}
