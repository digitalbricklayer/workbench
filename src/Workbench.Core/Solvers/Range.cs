namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Range of a domain.
    /// </summary>
    public class Range
    {
        /// <summary>
        /// Initialize a range with an upper and lower band.
        /// </summary>
        /// <param name="lowerBand">Lower band.</param>
        /// <param name="upperBand">Upper band.</param>
        public Range(long lowerBand, long upperBand)
        {
            Upper = upperBand;
            Lower = lowerBand;
        }

        /// <summary>
        /// Gets the lower range.
        /// </summary>
        public long Lower { get; private set; }

        /// <summary>
        /// Gets the upper range.
        /// </summary>
        public long Upper { get; private set; }
    }
}