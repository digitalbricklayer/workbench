using System;
using System.Diagnostics;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class DomainExpressionEvaluator
    {
        internal DomainExpressionEvaluatorContext Context { get; private set; }

        private DomainExpressionEvaluator(DomainExpressionEvaluatorContext theContext)
        {
            Context = theContext;
        }

        public static DomainRange Evaluate(DomainExpressionEvaluatorContext theContext)
        {
            var evaluator = new DomainExpressionEvaluator(theContext);
            return evaluator.Evaluate();
        }

        public DomainRange Evaluate()
        {
            var theDomainExpression = Context.DomainExpression;

            var lhsBand = EvaluateBand(theDomainExpression.LeftExpression);
            var rhsBand = EvaluateBand(theDomainExpression.RightExpression);
            return new DomainRange(lhsBand, rhsBand);
        }

        private long EvaluateBand(BandExpressionNode theExpression)
        {
            if (theExpression.Inner is NumberLiteralNode numberLiteral)
            {
                return numberLiteral.Value;
            }

            if (theExpression.Inner is FunctionInvocationNode functionCall)
            {
                return EvaluateSizeFunction(functionCall);
            }

            throw new NotImplementedException("Unknown band expression node.");
        }

        private long EvaluateSizeFunction(FunctionInvocationNode functionCall)
        {
            Debug.Assert(functionCall.FunctionName == "size");

            var variableName = functionCall.ArgumentList.Arguments.First().Value.Value;
            var theVariable = Context.Model.GetVariableByName(variableName);
            return theVariable.GetSize();
        }
    }
}