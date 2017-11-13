namespace Workbench.Core.Solver
{
    /// <summary>
    /// Range of a domain.
    /// </summary>
    public class Range
    {
        /// <summary>
        /// Initialize a range with an upper and lower band.
        /// </summary>
        /// <param name="upperBand">Upper band.</param>
        /// <param name="lowerBand">Lower band.</param>
        public Range(long upperBand, long lowerBand)
        {
            Upper = upperBand;
            Lower = lowerBand;
        }

        /// <summary>
        /// Gets the lower band.
        /// </summary>
        public long Lower { get; private set; }

        /// <summary>
        /// Gets the upper band.
        /// </summary>
        public long Upper { get; private set; }
    }
}