using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Range of values for a domain.
    /// </summary>
    public class RangeDomainValue : DomainValue
    {
        /// <summary>
        /// Initialize a domain range with a low and high band.
        /// </summary>
        /// <param name="low">Low band.</param>
        /// <param name="high">High band.</param>
        internal RangeDomainValue(long low, long high)
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

        public override Range GetRange()
        {
            return new Range(Lower, Upper);
        }

        /// <summary>
        /// Does the domain value intersect with this range.
        /// </summary>
        /// <param name="theDomainValue">Domain value.</param>
        /// <returns>True if intersects. False if it does not intersect.</returns>
        public override bool IntersectsWith(DomainValue theDomainValue)
        {
            Contract.Requires<ArgumentNullException>(theDomainValue != null);

            var otherModel = (RangeDomainValue) theDomainValue;
            return otherModel.Upper <= Upper && otherModel.Lower >= Lower;
        }
    }
}
