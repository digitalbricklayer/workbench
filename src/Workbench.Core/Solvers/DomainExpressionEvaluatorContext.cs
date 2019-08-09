using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal class DomainExpressionEvaluatorContext
    {
        internal VariableDomainExpressionNode DomainExpression { get; private set; }
        internal ModelModel Model { get; private set; }

        internal DomainExpressionEvaluatorContext(VariableDomainExpressionNode theDomainExpression, ModelModel theModel)
        {
            DomainExpression = theDomainExpression;
            Model = theModel;
        }
    }
}