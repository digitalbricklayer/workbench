using System;
using System.Collections.Generic;
using System.Linq;

namespace Dyna.Core.Models
{
    /// <summary>
    /// A domain contains the possible values that a variable can be bound to.
    /// </summary>
    [Serializable]
    public class DomainModel : GraphicModel
    {
        private readonly List<int> values = new List<int>();
        private DomainExpressionModel expression;

        public DomainModel(string domainName, string rawDomainExpression)
            : base(domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentException("domainName");
            if (string.IsNullOrWhiteSpace(rawDomainExpression))
                throw new ArgumentException("rawDomainExpression");
            this.ParseExpression(rawDomainExpression);
        }

        public DomainModel(params int[] theRange)
        {
            if (theRange == null)
                throw new ArgumentNullException("theRange");
            this.values.AddRange(theRange);
        }

        public DomainModel(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
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
            this.Expression = new DomainExpressionModel(DomainGrammar.Parse(rawExpression));
            var theRange = Enumerable.Range(this.Expression.LowerBand, this.Expression.Size);
            this.values.AddRange(theRange);
        }
    }
}
