using System;
using System.Collections.Generic;
using System.Linq;

namespace DynaApp.Entities
{
    /// <summary>
    /// A domain contains the possible values that a variable can be bound to.
    /// </summary>
    class Domain
    {
        private readonly List<int> values = new List<int>();

        public Domain(string domainName, string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                throw new ArgumentException("domainName");
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Name = domainName;
            this.ParseExpression(rawExpression);
        }

        public Domain(params int[] theRange)
        {
            if (theRange == null)
                throw new ArgumentNullException("theRange");
            this.values.AddRange(theRange);
        }

        public Domain(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.ParseExpression(rawExpression);
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

        /// <summary>
        /// Gets or sets the model the constraint is a part of.
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        /// Gets or sets the range expression.
        /// </summary>
        public RangeExpression Expression { get; private set; }

        /// <summary>
        /// Gets the domain name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Create a new Domain from a range of individual values.
        /// </summary>
        /// <param name="theRange">The individual values.</param>
        /// <returns>New domain.</returns>
        public static Domain CreateFrom(params int[] theRange)
        {
            return new Domain(theRange);
        }

        /// <summary>
        /// Parse a raw expression.
        /// </summary>
        /// <param name="rawExpression">Raw domain expression.</param>
        public void ParseExpression(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException("rawExpression");
            this.Expression = DomainGrammar.Parse(rawExpression);
            var theRange = Enumerable.Range(this.Expression.LowerBand, this.Expression.Size);
            this.values.AddRange(theRange);
        }
    }
}
