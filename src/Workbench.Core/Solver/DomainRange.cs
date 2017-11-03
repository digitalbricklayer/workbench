using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Range of values of a domain.
    /// </summary>
    public class DomainRange
    {
        /// <summary>
        /// Initialize a domain range with a low and high band.
        /// </summary>
        /// <param name="low">Low band.</param>
        /// <param name="high">High band.</param>
        internal DomainRange(long low, long high)
        {
            Lower = low;
            Upper = high;
        }

        /// <summary>
        /// Gets the lower band.
        /// </summary>
        public long Lower { get; private set; }

        /// <summary>
        /// Gets the upper band.
        /// </summary>
        public long Upper { get; private set; }

        /// <summary>
        /// Does the otherModel intersect with this range.
        /// </summary>
        /// <param name="otherModel"></param>
        /// <returns>True if intersects. False if it does not intersect.</returns>
        public bool IntersectsWith(DomainRange otherModel)
        {
            Contract.Requires<ArgumentNullException>(otherModel != null);

            return otherModel.Upper <= Upper && otherModel.Lower >= Lower;
        }
    }
}
