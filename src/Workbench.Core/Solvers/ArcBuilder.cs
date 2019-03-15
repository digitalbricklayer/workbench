using System;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class ArcBuilder
    {
        private readonly OrangeCache _cache;

        internal ArcBuilder(OrangeCache cache)
        {
            _cache = cache;
        }

        internal Arc Build(ExpressionConstraintModel expressionConstraint)
        {
            return new Arc(GetVariableFrom(expressionConstraint.Expression.Node.InnerExpression.LeftExpression),
                      GetVariableFrom(expressionConstraint.Expression.Node.InnerExpression.RightExpression), 
                           CreateConstraintFrom(expressionConstraint.Expression.Node));
        }

        private IntegerVariable GetVariableFrom(ExpressionNode expressionNode)
        {
            switch (expressionNode.InnerExpression)
            {
                case SingletonVariableReferenceNode singletonVariableReference:
                    return _cache.GetSolverSingletonVariableByName(singletonVariableReference.VariableName);

                case AggregateVariableReferenceNode aggregateVariableReference:
                    return _cache.GetSolverAggregateVariableByName(aggregateVariableReference.VariableName,
                                                                   aggregateVariableReference.SubscriptStatement.Subscript);

                default:
                    throw new NotImplementedException();
            }
        }

        private ConstraintExpression CreateConstraintFrom(ConstraintExpressionNode binaryExpressionNode)
        {
            return new ConstraintExpression(binaryExpressionNode);
        }
    }
}