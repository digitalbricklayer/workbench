using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solver
{
    internal class CounterContext
    {
        public CounterContext(string counterName, CounterRange theRange)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(counterName));
            Contract.Requires<ArgumentNullException>(theRange != null);
            CounterName = counterName;
            Range = theRange;
            CurrentValue = theRange.Low;
        }

        public string CounterName { get; private set; }
        public int CurrentValue { get; private set; }
        public CounterRange Range { get; private set; }

        /// <summary>
        /// Move to the next value in the range.
        /// </summary>
        /// <returns>True if another value is available. False if the end of 
        /// the range has been reached.</returns>
        public bool Next()
        {
            if (CurrentValue == Range.High) return false;
            CurrentValue++;
            return true;
        }
    }
}