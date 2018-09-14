using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    internal class VariableBandEvaluator
    {
        internal static DomainValue GetVariableBand(VariableModel theVariable)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            Contract.Assert(!theVariable.DomainExpression.IsEmpty);

            var variableDomainExpressionRoot = theVariable.DomainExpression.Node;
            var evaluatorContext = new VariableDomainExpressionEvaluatorContext(variableDomainExpressionRoot, theVariable.Model.Workspace);
            return VariableDomainExpressionEvaluator.Evaluate(evaluatorContext);
        }
    }
}
