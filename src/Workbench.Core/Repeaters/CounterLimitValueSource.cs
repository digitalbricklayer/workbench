using System;
using System.Diagnostics;

namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Implementation of the limit value source for a limit tied to the 
    /// current value of a counter.
    /// </summary>
    internal sealed class CounterLimitValueSource : ILimitValueSource
    {
        private readonly CounterContext context;

        /// <summary>
        /// Initialize the counter scope limit value with a counter context.
        /// </summary>
        /// <param name="theContext">Counter context.</param>
        public CounterLimitValueSource(CounterContext theContext)
        {
            this.context = theContext;
        }

        /// <summary>
        /// Get the current value of the scope limit.
        /// </summary>
        /// <returns>Current scope limit value.</returns>
        public int GetValue()
        {
            Debug.Assert(this.context != null);
            return this.context.CurrentValue;
        }
    }
}
