using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class SharedDomainExpressionEvaluatorContext
    {
        public VariableDomainExpressionNode DomainExpression { get; private set; }
        public ModelModel Model { get; private set; }

        public SharedDomainExpressionEvaluatorContext(VariableDomainExpressionNode theVariableDomainExpression, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theVariableDomainExpression != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            DomainExpression = theVariableDomainExpression;
            Model = theModel;
        }
    }
}