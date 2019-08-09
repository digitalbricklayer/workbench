using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal class SharedDomainExpressionEvaluatorContext
    {
        internal SharedDomainExpressionNode DomainExpression { get; private set; }
        internal ModelModel Model => Workspace.Model;
        internal WorkspaceModel Workspace { get; private set; }

        internal SharedDomainExpressionEvaluatorContext(SharedDomainExpressionNode theExpressionNode, WorkspaceModel theWorkspace)
        {
            DomainExpression = theExpressionNode;
            Workspace = theWorkspace;
        }
    }
}