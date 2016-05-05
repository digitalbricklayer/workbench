using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Tracks the value of a counter.
    /// </summary>
    internal class CounterContext
    {
        public CounterContext(string theCounterName, CounterRange theRange)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theCounterName));
            Contract.Requires<ArgumentNullException>(theRange != null);
            CounterName = theCounterName;
            Range = theRange;
            Reset();
        }

        /// <summary>
        /// Gets the counter name.
        /// </summary>
        public string CounterName { get; private set; }

        /// <summary>
        /// Gets the counter's current value.
        /// </summary>
        public int CurrentValue { get; private set; }

        /// <summary>
        /// Gets the counter range of values.
        /// </summary>
        public CounterRange Range { get; private set; }

        /// <summary>
        /// Advance the counter to the next value of the range.
        /// </summary>
        /// <returns>True if another value is available. False if the end of 
        /// the range has been reached.</returns>
        public bool Next()
        {
            if (CurrentValue == Range.High) return false;
            CurrentValue++;

            return true;
        }

        /// <summary>
        /// Reset the current value to its initial position.
        /// </summary>
        public void Reset()
        {
            CurrentValue = Range.Low - 1;
        }
    }
}
