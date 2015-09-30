using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// A variable domain expression model can be either be a reference to a shared 
    /// domain or an inline domain expression.
    /// </summary>
    [Serializable]
    public class VariableDomainExpressionModel
    {
        /// <summary>
        /// Initialize a variable domain expression with raw expression text.
        /// </summary>
        public VariableDomainExpressionModel(string rawExpression)
        {
            this.Text = rawExpression;
        }

        /// <summary>
        /// Initialize a variable domain expression with a shared domain reference.
        /// </summary>
        /// <param name="sharedDomainReference">Shared domain reference.</param>
        public VariableDomainExpressionModel(SharedDomainReference sharedDomainReference)
        {
            this.DomainReference = sharedDomainReference;
        }

        /// <summary>
        /// Initialize a variable domain expression with an inline domain expression.
        /// </summary>
        /// <param name="domainExpression">Inline domain expression.</param>
        public VariableDomainExpressionModel(DomainExpressionModel domainExpression)
        {
            this.InlineDomain = domainExpression;
        }

        /// <summary>
        /// Initialize a variable domain expression with default values.
        /// </summary>
        public VariableDomainExpressionModel()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the variable domain expression text.
        /// </summary>
        public string Text { get; set; }

        public SharedDomainReference DomainReference { get; private set; }

        public DomainExpressionModel InlineDomain { get; private set; }

        /// <summary>
        /// Gets whether the domain expression has a value.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.InlineDomain == null && this.DomainReference == null;
            }
        }
    }
}