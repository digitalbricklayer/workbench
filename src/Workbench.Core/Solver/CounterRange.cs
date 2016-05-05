using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class CounterRange
    {
        public CounterRange(ILimitValueSource lowValueSource, ILimitValueSource highValueSource)
        {
            Contract.Requires<ArgumentNullException>(lowValueSource != null);
            Contract.Requires<ArgumentNullException>(highValueSource != null);
            Low = lowValueSource;
            High = highValueSource;
        }

        public ILimitValueSource Low { get; private set; }
        public ILimitValueSource High { get; private set; }
    }

    /// <summary>
    /// Contract for getting a literal or currnet counter value.
    /// </summary>
    internal interface ILimitValueSource
    {
        /// <summary>
        /// Get the current value of the scope limit.
        /// </summary>
        /// <returns>Current scope limit value.</returns>
        int GetValue();
    }

    /// <summary>
    /// Implementation of the limit value source for a literal.
    /// </summary>
    internal sealed class LiteralLimitValueSource : ILimitValueSource
    {
        private readonly LiteralNode node;

        /// <summary>
        /// Initialize the literal value source with a literal node.
        /// </summary>
        /// <param name="theNode">Literal node.</param>
        public LiteralLimitValueSource(LiteralNode theNode)
        {
            Contract.Requires<ArgumentNullException>(theNode != null);
            this.node = theNode;
        }

        /// <summary>
        /// Get the current value of the scope limit.
        /// </summary>
        /// <returns>Current scope limit value.</returns>
        public int GetValue()
        {
            Contract.Assume(this.node != null);
            return this.node.Value;
        }
    }

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
            Contract.Requires<ArgumentNullException>(theContext != null);
            this.context = theContext;
        }

        /// <summary>
        /// Get the current value of the scope limit.
        /// </summary>
        /// <returns>Current scope limit value.</returns>
        public int GetValue()
        {
            Contract.Assume(this.context != null);
            return this.context.CurrentValue;
        }
    }
}
