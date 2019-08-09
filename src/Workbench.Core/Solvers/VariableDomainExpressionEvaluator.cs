using System;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal static class VariableDomainExpressionEvaluator
    {
        internal static DomainValue Evaluate(VariableDomainExpressionEvaluatorContext theContext)
        {
            var theDomainExpression = theContext.DomainExpression;

            switch (theDomainExpression.Inner)
            {
                case SharedDomainReferenceNode domainReferenceNode:
                    return DomainExpressionEvaluator.Evaluate(domainReferenceNode, theContext.Workspace);

                case RangeDomainExpressionNode rangeExpressionNode:
                    return DomainExpressionEvaluator.Evaluate(rangeExpressionNode, theContext.Model);

                case ListDomainExpressionNode listDomainExpressionNode:
                    return DomainExpressionEvaluator.Evaluate(listDomainExpressionNode);

                case TableExpressionNode tableRangeNode:
                    return new DomainExpressionEvaluator().Evaluate(tableRangeNode, theContext.Workspace);

                default:
                    throw new NotImplementedException("Unknown variable domain expression.");
            }
        }
    }
}