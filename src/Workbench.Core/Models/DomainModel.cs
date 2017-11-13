using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public class DomainModel : AbstractModel
    {
        private DomainExpressionModel expression;

        /// <summary>
        /// Initialize a domain with a domain expression.
        /// </summary>
        public DomainModel(DomainExpressionModel theExpression)
        {
            Contract.Requires<ArgumentNullException>(theExpression != null);
            this.expression = theExpression;
        }

        /// <summary>
        /// Initialize a domain with a domain expression.
        /// </summary>
        public DomainModel(string rawDomainExpression)
            : this(new DomainExpressionModel(rawDomainExpression))
        {
        }

        /// <summary>
        /// Initialize a domain with default values.
        /// </summary>
        public DomainModel()
        {
            Expression = new DomainExpressionModel();
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public DomainExpressionModel Expression
        {
            get { return this.expression; }
            set
            {
                this.expression = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Parse a raw expression.
        /// </summary>
        /// <param name="rawExpression">Raw domain expression.</param>
        public void ParseExpression(string rawExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(rawExpression));
            Expression = new DomainExpressionModel(rawExpression);
        }
    }
}
