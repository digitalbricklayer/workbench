using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A domain contains the possible values that a variable can be bound to.
    /// </summary>
    [Serializable]
    public class DomainGraphicModel : GraphicModel
    {
        private DomainModel domain;

        public DomainGraphicModel(string domainName, Point location, DomainModel theDomain)
            : base(domainName, location)
        {
            if (theDomain == null)
                throw new ArgumentNullException(nameof(theDomain));
            Contract.EndContractBlock();
            this.domain = theDomain;
        }

        public DomainGraphicModel(string domainName, DomainExpressionModel theExpression)
            : base(domainName)
        {
            if (theExpression == null)
                throw new ArgumentNullException(nameof(theExpression));
            Contract.EndContractBlock();
            this.domain = new DomainModel(theExpression);
        }

        public DomainGraphicModel(string domainName, string rawDomainExpression)
            : this(domainName, new DomainExpressionModel(rawDomainExpression))
        {
        }

        public DomainGraphicModel(string rawExpression)
            : this("Constraint", rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            Contract.EndContractBlock();
            ParseExpression(rawExpression);
        }

        public DomainGraphicModel()
            : base("New domain")
        {
            this.domain = new DomainModel();
        }

        public DomainExpressionModel Expression
        {
            get { return this.domain.Expression; }
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
            this.domain.ParseExpression(rawExpression);
        }
    }
}
