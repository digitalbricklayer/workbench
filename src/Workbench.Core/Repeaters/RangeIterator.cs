using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Iterator for a range of values.
    /// </summary>
    internal sealed class RangeIterator : ICounterIterator
    {
        /// <summary>
        /// Initialize a range iterator with a range start and end.
        /// </summary>
        /// <param name="startValueSource">Start value.</param>
        /// <param name="endValueSource">End value.</param>
        public RangeIterator(ILimitValueSource startValueSource, ILimitValueSource endValueSource)
        {
            Contract.Requires<ArgumentNullException>(startValueSource != null);
            Contract.Requires<ArgumentNullException>(endValueSource != null);
            Start = startValueSource;
            End = endValueSource;
            Reset();
        }

        /// <summary>
        /// Gets the counter's current value.
        /// </summary>
        public int CurrentValue { get; private set; }

        /// <summary>
        /// Gets the range start value.
        /// </summary>
        public ILimitValueSource Start { get; private set; }

        /// <summary>
        /// Gets the range end value.
        /// </summary>
        public ILimitValueSource End { get; private set; }

        /// <summary>
        /// Advance the counter to the next value of the range.
        /// </summary>
        /// <returns>True if another value is available. False if the end of 
        /// the range has been reached.</returns>
        public bool Next()
        {
            if (CurrentValue >= End.GetValue()) return false;
            CurrentValue++;

            return true;
        }

        /// <summary>
        /// Reset the current value to its initial position.
        /// </summary>
        public void Reset()
        {
            CurrentValue = Start.GetValue() - 1;
        }
    }
}
