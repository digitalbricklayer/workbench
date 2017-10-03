using System;
using System.Diagnostics;
using System.Linq;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class DomainExpressionEvaluator
    {
        internal static DomainExpressionEvaluatorContext Context { get; private set; }

        public static Tuple<long, long> Evaluate(DomainExpressionEvaluatorContext theContext)
        {
            Context = theContext;
            var theDomainExpression = theContext.DomainExpression;

            var lhsBand = EvaluateBand(theDomainExpression.LeftExpression);
            var rhsBand = EvaluateBand(theDomainExpression.RightExpression);
            return new Tuple<long, long>(lhsBand, rhsBand);
        }

        private static long EvaluateBand(BandExpressionNode theExpression)
        {
            var numberLiteral = theExpression.Inner as NumberLiteralNode;
            if (numberLiteral != null)
            {
                return numberLiteral.Value;
            }

            var functionCall = theExpression.Inner as FunctionCallXNode;
            if (functionCall != null)
            {
                return EvaluateSizeFunction(functionCall);
            }

            throw new NotImplementedException("Unknown band expression node.");
        }

        private static long EvaluateSizeFunction(FunctionCallXNode functionCall)
        {
            Debug.Assert(functionCall.FunctionName.Name == "size");

            var variableName = functionCall.ArgumentList.Arguments.First().Value.Value;
            var theVariable = Context.Model.GetVariableByName(variableName);
            return theVariable.GetSize();
        }
    }
}