using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class SharedDomainExpressionEvaluatorContext
    {
        public SharedDomainExpressionNode DomainExpression { get; private set; }
        public ModelModel Model { get; private set; }

        public SharedDomainExpressionEvaluatorContext(SharedDomainExpressionNode theExpressionNode, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            DomainExpression = theExpressionNode;
            Model = theModel;
        }
    }
}