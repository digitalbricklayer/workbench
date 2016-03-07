using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A domain contains the possible values that a variable can be bound to.
    /// </summary>
    [Serializable]
    public class DomainModel : GraphicModel
    {
        private readonly List<int> values = new List<int>();
        private DomainExpressionModel expression;

        public DomainModel(string domainName, Point location, DomainExpressionModel theExpression)
            : base(domainName, location)
        {
            if (theExpression == null)
                throw new ArgumentNullException("theExpression");
            Contract.EndContractBlock();
            this.expression = theExpression;
        }

        public DomainModel(string domainName, DomainExpressionModel theExpression)
            : base(domainName)
        {
            if (theExpression == null)
                throw new ArgumentNullException("theExpression");
            Contract.EndContractBlock();
            this.expression = theExpression;
        }

        public DomainModel(string domainName, string rawDomainExpression)
            : this(domainName, new DomainExpressionModel(rawDomainExpression))
        {
        }

        public DomainModel(params int[] theRange)
        {
            if (theRange == null)
                throw new ArgumentNullException("theRange");
            Contract.EndContractBlock();
            this.values.AddRange(theRange);
        }

        public DomainModel(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            Contract.EndContractBlock();
            this.ParseExpression(rawExpression);
        }

        public DomainModel()
            : base("New domain")
        {
            this.Expression = new DomainExpressionModel();
        }

        /// <summary>
        /// Gets the domain values.
        /// </summary>
        public IEnumerable<int> Values
        {
            get
            {
                return this.values;
            }
        }

        public DomainExpressionModel Expression
        {
            get { return expression; }
            set
            {
                expression = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Create a new Domain from a range of individual values.
        /// </summary>
        /// <param name="theRange">The individual values.</param>
        /// <returns>New domain.</returns>
        public static DomainModel CreateFrom(params int[] theRange)
        {
            return new DomainModel(theRange);
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
            this.Expression = new DomainExpressionModel(DomainGrammar.Parse(rawExpression));
            var theRange = Enumerable.Range(this.Expression.LowerBand, this.Expression.Size);
            this.values.AddRange(theRange);
        }
    }
}
