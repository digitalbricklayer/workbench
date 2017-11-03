using System;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal static class VariableDomainExpressionEvaluator
    {
        internal static DomainRange Evaluate(VariableDomainExpressionEvaluatorContext theContext)
        {
            var theDomainExpression = theContext.DomainExpression;

            switch (theDomainExpression.Inner)
            {
                case SharedDomainReferenceNode domainReferenceNode:
                    var x = new SharedDomainExpressionEvaluatorContext(theDomainExpression, theContext.Model);
                    return SharedDomainExpressionEvaluator.Evaluate(x);

                case DomainExpressionNode domainExpressionNode:
                    var y = new DomainExpressionEvaluatorContext(domainExpressionNode, theContext.Model);
                    return DomainExpressionEvaluator.Evaluate(y);

                default:
                    throw new NotImplementedException("Unknown variable domain expression.");
            }
        }
    }
}