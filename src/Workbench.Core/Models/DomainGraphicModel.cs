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
        private readonly DomainModel _domain;

        public DomainGraphicModel(DomainModel theDomain, Point location)
            : base(theDomain, location)
        {
            Contract.Requires<ArgumentNullException>(theDomain != null);
            _domain = theDomain;
        }

        public DomainGraphicModel(DomainModel theDomain)
            : base(theDomain, new Point())
        {
            Contract.Requires<ArgumentNullException>(theDomain != null);
            _domain = theDomain;
        }

        public DomainExpressionModel Expression
        {
            get { return _domain.Expression; }
        }

        public DomainModel Domain
        {
            get { return _domain; }
        }

        /// <summary>
        /// Parse a raw expression.
        /// </summary>
        /// <param name="rawExpression">Raw domain expression.</param>
        public void ParseExpression(string rawExpression)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(rawExpression));
            _domain.ParseExpression(rawExpression);
        }
    }
}
