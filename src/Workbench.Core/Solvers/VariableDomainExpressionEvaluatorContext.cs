using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal class VariableDomainExpressionEvaluatorContext
    {
        internal VariableDomainExpressionNode DomainExpression { get; private set; }
        internal ModelModel Model => Workspace.Model;
        internal WorkspaceModel Workspace { get; private set; }

        internal VariableDomainExpressionEvaluatorContext(VariableDomainExpressionNode theDomainExpression, WorkspaceModel theWorkspace)
        {
            DomainExpression = theDomainExpression;
            Workspace = theWorkspace;
        }
    }
}