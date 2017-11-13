using System;
using System.Diagnostics;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class RangeDomainExpressionEvaluator
    {
        private SharedDomainExpressionEvaluatorContext context;

        private RangeDomainExpressionEvaluator(SharedDomainExpressionEvaluatorContext context)
        {
            Context = context;
        }

        internal RangeDomainExpressionEvaluatorContext Context
        {
            get { return this.context; }
            private set { this.context = value; }
        }

        public static RangeDomainValue Evaluate(RangeDomainExpressionEvaluatorContext theContext)
        {
            var evaluator = new RangeDomainExpressionEvaluator(theContext);
            return evaluator.Evaluate();
        }

        private RangeDomainValue Evaluate()
        {
            var theDomainExpression = Context.DomainExpression;

            switch (theDomainExpression.Inner)
            {
                case RangeDomainExpressionNode rangeDomainExpression:
                    return EvaluateRangeNodeExpression(rangeDomainExpression);

                case ListDomainExpressionNode listDomainExpressionNode:
                    return EvaluateListDomainExpression(listDomainExpressionNode);

                case SharedDomainReferenceNode sharedDomainReferenceNode:
                    var sharedDomainName = sharedDomainReferenceNode.DomainName;
                    var sharedDomainModel = Context.Model.GetSharedDomainByName(sharedDomainName.Name);

                    var evaluatorContext = new DomainExpressionEvaluatorContext(sharedDomainReferenceNode, Context.Model);
                    return DomainExpressionEvaluator.Evaluate(evaluatorContext);

                default:
                    throw new NotImplementedException("Unknown domain expression.");
            }

            throw new NotImplementedException("Unknown variable domain expression.");
        }

        private RangeDomainValue EvaluateListDomainExpression(ListDomainExpressionNode listDomainExpressionNode)
        {
            return null;
        }

        private RangeDomainValue EvaluateRangeNodeExpression(RangeDomainExpressionNode theDomainExpression)
        {
            var lhsBand = EvaluateBand(theDomainExpression.LeftExpression);
            var rhsBand = EvaluateBand(theDomainExpression.RightExpression);
            return new RangeDomainValue(lhsBand, rhsBand);
        }

        private long EvaluateBand(BandExpressionNode theExpression)
        {
            var numberLiteral = theExpression.Inner as NumberLiteralNode;
            if (numberLiteral != null)
            {
                return numberLiteral.Value;
            }

            var functionCall = theExpression.Inner as FunctionInvocationNode;
            if (functionCall != null)
            {
                Debug.Assert(functionCall.FunctionName == "size");

                var variableName = functionCall.FunctionName;
                var theVariable = Context.Model.GetVariableByName(variableName);
                return theVariable.GetSize();
            }

            throw new NotImplementedException("Unknown band expression node.");
        }
    }
}