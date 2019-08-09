using System;

namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Tracks the value of a counter.
    /// </summary>
    internal class CounterContext : ICounterIterator
    {
        public CounterContext(string theCounterName, ICounterIterator theCounter)
        {
            if (string.IsNullOrWhiteSpace(theCounterName))
                throw new ArgumentException("Counter name cannot be blank", nameof(theCounter));

            CounterName = theCounterName;
            Counter = theCounter;
        }

        /// <summary>
        /// Gets the counter name.
        /// </summary>
        public string CounterName { get; private set; }

        /// <summary>
        /// Gets the counter range of values.
        /// </summary>
        public ICounterIterator Counter { get; private set; }

        /// <summary>
        /// Gets the counter's current value.
        /// </summary>
        public int CurrentValue => Counter.CurrentValue;

        /// <summary>
        /// Advance the counter to the next value of the range.
        /// </summary>
        /// <returns>True if another value is available. False if the end of 
        /// the range has been reached.</returns>
        public bool Next()
        {
            return Counter.Next();
        }

        /// <summary>
        /// Reset the current value to its default position.
        /// </summary>
        public void Reset()
        {
            Counter.Reset();
        }
    }
}
