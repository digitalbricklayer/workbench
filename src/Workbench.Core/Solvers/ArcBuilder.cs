using System;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class ArcBuilder
    {
        private readonly Ac1Cache _cache;

        internal ArcBuilder(Ac1Cache cache)
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
                    return _cache.GetVariableByName(singletonVariableReference.VariableName);

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