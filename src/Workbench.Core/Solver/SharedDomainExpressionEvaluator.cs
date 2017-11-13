using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class SharedDomainExpressionEvaluator
    {
        private SharedDomainExpressionEvaluator(SharedDomainExpressionEvaluatorContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null);

            Context = context;
        }

        internal SharedDomainExpressionEvaluatorContext Context { get; private set; }

        public static DomainValue Evaluate(SharedDomainExpressionEvaluatorContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theContext != null);

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

                default:
                    throw new NotImplementedException("Unknown domain expression.");
            }
        }
    }
}