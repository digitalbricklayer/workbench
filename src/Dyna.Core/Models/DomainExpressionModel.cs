using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// An expression specifying a domain.
    /// </summary>
    /// <remarks>
    /// Can be either a name referencing a seperate shared 
    /// domain, or an expression specifying a domain for the variable.
    /// </remarks>
    [Serializable]
    public class DomainExpressionModel
    {
        /// <summary>
        /// Initialize a domain expression with a raw domain expression text.
        /// </summary>
        public DomainExpressionModel(string rawDomainExpression)
        {
            this.Text = rawDomainExpression;
        }

        /// <summary>
        /// Initialize a domain expression with an upper and lower band for the domain.
        /// </summary>
        /// <param name="upperBand">Domain upper band.</param>
        /// <param name="lowerBand">Domain lower band.</param>
        public DomainExpressionModel(int upperBand, int lowerBand)
        {
            this.UpperBand = upperBand;
            this.LowerBand = lowerBand;
        }

        /// <summary>
        /// Initialize a domain expression with default values.
        /// </summary>
        public DomainExpressionModel()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the expression text.
        /// </summary>
        public string Text { get; set; }

        public int UpperBand { get; private set; }
        public int LowerBand { get; private set; }

        /// <summary>
        /// Gets the size of the range.
        /// </summary>
        public int Size
        {
            get
            {
                return this.UpperBand - this.LowerBand + 1;
            }
        }
    }
}