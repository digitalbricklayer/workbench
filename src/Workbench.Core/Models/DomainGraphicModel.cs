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

        public DomainGraphicModel(DomainModel theDomain, Point location)
            : base(theDomain, location)
        {
            Contract.Requires<ArgumentNullException>(theDomain != null);
            this.domain = theDomain;
        }

        public DomainGraphicModel(DomainModel theDomain)
            : base(theDomain, new Point())
        {
            Contract.Requires<ArgumentNullException>(theDomain != null);
            this.domain = theDomain;
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
