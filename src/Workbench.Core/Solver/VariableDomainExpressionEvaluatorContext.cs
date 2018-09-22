using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class VariableDomainExpressionEvaluatorContext
    {
        internal VariableDomainExpressionNode DomainExpression { get; private set; }
        internal ModelModel Model => Workspace.Model;
        internal WorkspaceModel Workspace { get; private set; }

        internal VariableDomainExpressionEvaluatorContext(VariableDomainExpressionNode theDomainExpression, WorkspaceModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            DomainExpression = theDomainExpression;
            Workspace = theWorkspace;
        }
    }
}