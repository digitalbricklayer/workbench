using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public class DomainExpressionUnit
    {
        /// <summary>
        /// Initialize a domain expression unit with an upper and lower band for the domain.
        /// </summary>
        /// <param name="upperBand">Domain upper band.</param>
        /// <param name="lowerBand">Domain lower band.</param>
        public DomainExpressionUnit(int upperBand, int lowerBand)
        {
            this.UpperBand = upperBand;
            this.LowerBand = lowerBand;
        }

        public DomainExpressionUnit()
        {
            
        }

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
