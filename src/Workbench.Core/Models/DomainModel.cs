using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public class DomainModel : AbstractModel
    {
        private DomainExpressionModel expression;

        public DomainModel(DomainExpressionModel theExpression)
        {
            if (theExpression == null)
                throw new ArgumentNullException(nameof(theExpression));
            Contract.EndContractBlock();
            this.expression = theExpression;
        }

        public DomainModel(string rawDomainExpression)
            : this(new DomainExpressionModel(rawDomainExpression))
        {
        }

        public DomainModel()
        {
            Expression = new DomainExpressionModel();
        }

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
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            Contract.EndContractBlock();
            Expression = new DomainExpressionModel(rawExpression);
        }
    }
}
