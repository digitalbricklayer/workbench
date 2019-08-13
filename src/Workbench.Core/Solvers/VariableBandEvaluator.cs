using System.Diagnostics;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal static class VariableBandEvaluator
    {
        internal static DomainValue GetVariableBand(VariableModel theVariable)
        {
            Debug.Assert(!theVariable.DomainExpression.IsEmpty);

            var variableDomainExpressionRoot = theVariable.DomainExpression.Node;
            var evaluatorContext = new VariableDomainExpressionEvaluatorContext(variableDomainExpressionRoot, theVariable.Parent.Workspace);
            return VariableDomainExpressionEvaluator.Evaluate(evaluatorContext);
        }
    }
}
