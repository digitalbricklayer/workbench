using System;
using System.Diagnostics;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class SharedDomainExpressionEvaluator
    {
        private SharedDomainExpressionEvaluatorContext context;

        private SharedDomainExpressionEvaluator(SharedDomainExpressionEvaluatorContext context)
        {
            Context = context;
        }

        internal SharedDomainExpressionEvaluatorContext Context
        {
            get { return this.context; }
            private set { this.context = value; }
        }

        public static Tuple<long, long> Evaluate(SharedDomainExpressionEvaluatorContext theContext)
        {
            var evaluator = new SharedDomainExpressionEvaluator(theContext);
            return evaluator.Evaluate();
        }

        private Tuple<long, long> Evaluate()
        {
            var theDomainExpression = Context.DomainExpression;

            if (theDomainExpression.InlineDomain != null)
            {
                var lhsBand = EvaluateBand(theDomainExpression.InlineDomain.LeftExpression);
                var rhsBand = EvaluateBand(theDomainExpression.InlineDomain.RightExpression);
                return new Tuple<long, long>(lhsBand, rhsBand);
            }

            if (theDomainExpression.DomainReference != null)
            {
                var sharedDomainName = theDomainExpression.DomainReference.DomainName;
                var sharedDomainModel = Context.Model.GetSharedDomainByName(sharedDomainName.Name);

                var evaluatorContext = new DomainExpressionEvaluatorContext(sharedDomainModel.Expression.Node, Context.Model);
                return DomainExpressionEvaluator.Evaluate(evaluatorContext);
            }

            throw new NotImplementedException("Unknown variable domain expression.");
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