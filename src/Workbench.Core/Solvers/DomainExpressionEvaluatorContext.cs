using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            DomainExpression = theDomainExpression;
            Model = theModel;
        }
    }
}