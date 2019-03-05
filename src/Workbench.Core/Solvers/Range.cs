namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Range of a domain.
    /// </summary>
    public sealed class Range
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
        public long Lower { get; }

        /// <summary>
        /// Gets the upper range.
        /// </summary>
        public long Upper { get; }

        /// <summary>
        /// Gets the number of elements in the range.
        /// </summary>
        public long Count => Upper - Lower + 1;
    }
}