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
        public WorkspaceModel Workspace { get; private set; }

        internal VariableDomainExpressionEvaluatorContext(VariableDomainExpressionNode theDomainExpression, WorkspaceModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            DomainExpression = theDomainExpression;
            Workspace = theModel;
        }
    }
}