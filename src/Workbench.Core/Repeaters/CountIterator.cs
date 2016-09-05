using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Iterate a fixed number of times.
    /// </summary>
    internal sealed class CountIterator : ICounterIterator
    {
        /// <summary>
        /// Initialize a new count iterator with a count value source.
        /// </summary>
        /// <param name="countValueSource">High value source.</param>
        public CountIterator(ILimitValueSource countValueSource)
        {
            Contract.Requires<ArgumentNullException>(countValueSource != null);
            Count = countValueSource;
            Reset();
        }

        public ILimitValueSource Count { get; private set; }

        /// <summary>
        /// Gets the counter's current value.
        /// </summary>
        public int CurrentValue { get; private set; }

        /// <summary>
        /// Advance the counter to the next value of the range.
        /// </summary>
        /// <returns>True if another value is available. False if the end of 
        /// the range has been reached.</returns>
        public bool Next()
        {
            if (CurrentValue >= Count.GetValue() - 1) return false;
            CurrentValue++;

            return true;
        }

        /// <summary>
        /// Reset the current value to its initial position.
        /// </summary>
        public void Reset()
        {
            // The counter always starts at zero...
            CurrentValue = -1;
        }
    }
}