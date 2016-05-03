using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    internal class RepeaterContext
    {
        private CounterContext counter;

        /// <summary>
        /// Initialize a repeater context from the constraint.
        /// </summary>
        /// <param name="theConstraint">Expression constraint.</param>
        public RepeaterContext(ExpressionConstraintModel theConstraint)
        {
            Contract.Requires<ArgumentNullException>(theConstraint != null);
            this.Constraint = theConstraint;
            CreateCounterFrom(theConstraint);
        }

        /// <summary>
        /// Gets the counter contexts.
        /// </summary>
        /// <remarks>Will be null if no repeater is present.</remarks>
        public CounterContext Counter => this.counter;

        /// <summary>
        /// Gets whether there are any repeaters.
        /// </summary>
        public bool HasRepeaters => Counter != null;

        /// <summary>
        /// Gets the constraint.
        /// </summary>
        public ExpressionConstraintModel Constraint { get; private set; }

        private void CreateCounterFrom(ExpressionConstraintModel theConstraint)
        {
            if (!theConstraint.Expression.Node.HasExpander) return;
            AddCounter(new CounterContext(theConstraint.Expression.Node.Expander.CounterDeclaration.CounterName,
                                          new CounterRange(theConstraint.Expression.Node.Expander.ExpanderScope.Start.Value,
                                                           theConstraint.Expression.Node.Expander.ExpanderScope.End.Value)));
        }

        private void AddCounter(CounterContext newCounterContext)
        {
            Contract.Requires<ArgumentNullException>(newCounterContext != null);
            Contract.Assume(this.counter == null);
            this.counter = newCounterContext;
        }
    }
}