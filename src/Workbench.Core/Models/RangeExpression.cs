namespace Workbench.Core.Models
{
    public class RangeExpression
    {
        public RangeExpression(int upperBand, int lowerBand)
        {
            this.UpperBand = upperBand;
            this.LowerBand = lowerBand;
        }

        public RangeExpression()
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