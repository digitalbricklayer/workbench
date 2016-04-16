using System;
using Workbench.Core.Models;

namespace Workbench.Core.Expressions
{
    [Serializable]
    public class VariableDomainExpressionUnit
    {
        /// <summary>
        /// Initialize a variable domain expression with a shared domain reference.
        /// </summary>
        /// <param name="sharedDomainReference">Shared domain reference.</param>
        public VariableDomainExpressionUnit(SharedDomainReference sharedDomainReference)
        {
            this.DomainReference = sharedDomainReference;
        }

        /// <summary>
        /// Initialize a variable domain expression with an inline domain expression.
        /// </summary>
        /// <param name="domainExpression">Inline domain expression.</param>
        public VariableDomainExpressionUnit(DomainExpressionModel domainExpression)
        {
            this.InlineDomain = domainExpression;
        }

        public VariableDomainExpressionUnit()
        {
        }

        public SharedDomainReference DomainReference { get; private set; }

        public DomainExpressionModel InlineDomain { get; private set; }
    }
}
