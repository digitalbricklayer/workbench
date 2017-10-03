using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A domain contains the possible values that a variable can be bound to.
    /// </summary>
    [Serializable]
    public class DomainModel : GraphicModel
    {
        private DomainExpressionModel expression;

        public DomainModel(string domainName, Point location, DomainExpressionModel theExpression)
            : base(domainName, location)
        {
            if (theExpression == null)
                throw new ArgumentNullException(nameof(theExpression));
            Contract.EndContractBlock();
            this.expression = theExpression;
        }

        public DomainModel(string domainName, DomainExpressionModel theExpression)
            : base(domainName)
        {
            if (theExpression == null)
                throw new ArgumentNullException(nameof(theExpression));
            Contract.EndContractBlock();
            this.expression = theExpression;
        }

        public DomainModel(string domainName, string rawDomainExpression)
            : this(domainName, new DomainExpressionModel(rawDomainExpression))
        {
        }

        public DomainModel(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            Contract.EndContractBlock();
            ParseExpression(rawExpression);
        }

        public DomainModel()
            : base("New domain")
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
