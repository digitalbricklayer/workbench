using System;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal class SharedDomainExpressionEvaluator
    {
        private SharedDomainExpressionEvaluator(SharedDomainExpressionEvaluatorContext context)
        {
            Context = context;
        }

        internal SharedDomainExpressionEvaluatorContext Context { get; private set; }

        internal static DomainValue Evaluate(SharedDomainExpressionEvaluatorContext theContext)
        {
            var evaluator = new SharedDomainExpressionEvaluator(theContext);
            return evaluator.Evaluate();
        }

        private DomainValue Evaluate()
        {
            var theDomainExpression = Context.DomainExpression;

            switch (theDomainExpression.Inner)
            {
                case RangeDomainExpressionNode rangeDomainExpressionNode:
                    return DomainExpressionEvaluator.Evaluate(rangeDomainExpressionNode, Context.Model);

                case ListDomainExpressionNode listDomainExpressionNode:
                    return DomainExpressionEvaluator.Evaluate(listDomainExpressionNode);

                case TableExpressionNode tableRangeNode:
                    return new DomainExpressionEvaluator().Evaluate(tableRangeNode, Context.Workspace);

                default:
                    throw new NotImplementedException("Unknown domain expression.");
            }
        }
    }
}