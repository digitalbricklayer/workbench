using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class RepeaterContext
    {
        private readonly IList<CounterContext> counters;
        private bool firstIterationPassed;

        /// <summary>
        /// Initialize a repeater context from the constraint.
        /// </summary>
        /// <param name="theConstraint">Expression constraint.</param>
        public RepeaterContext(ExpressionConstraintModel theConstraint)
        {
            Contract.Requires<ArgumentNullException>(theConstraint != null);
            Constraint = theConstraint;
            this.counters = new List<CounterContext>();
            if (!theConstraint.Expression.Node.HasExpander) return;
            CreateCounterContextsFrom(theConstraint.Expression.Node.Expander);
        }

        public IReadOnlyCollection<CounterContext> Counters
        {
            get { return new ReadOnlyCollection<CounterContext>(counters); }
        }

        /// <summary>
        /// Gets whether there are any repeaters.
        /// </summary>
        public bool HasRepeaters => Counters.Any();

        /// <summary>
        /// Gets the constraint.
        /// </summary>
        public ExpressionConstraintModel Constraint { get; private set; }

        /// <summary>
        /// Move to the next counter value.
        /// </summary>
        /// <returns>True if a new counter value is available. False if no new counter values are available.</returns>
        public bool Next()
        {
            if (!firstIterationPassed)
            {
                InitializeAllCounters();
                firstIterationPassed = true;

                return true;
            }

            // Get the right most counter and go to the next iteration...
            for (var i = this.counters.Count - 1; i >= 0; i--)
            {
                var currentCounterStatus = this.counters[i].Next();
                if (currentCounterStatus) return true;
                
                // Is there another counter...
                if (i == 0)
                    break;

                // Try another counter...
                var previousCounter = this.counters[i - 1];
                var previousCounterStatus = previousCounter.Next();
                if (previousCounterStatus)
                {
                    counters[i].Reset();
                    counters[i].Next();
                    return true;
                }

                break;
            }

            return false;
        }

        private void CreateCounterContextsFrom(MultiRepeaterStatementNode theRepeater)
        {
            for (var i = 0; i < theRepeater.CounterDeclarations.CounterDeclarations.Count; i++)
            {
                var currentCounterDeclaration = theRepeater.CounterDeclarations.CounterDeclarations.ElementAt(i);
                var currentScopeDeclaration = theRepeater.ScopeDeclarations.ScopeDeclarations.ElementAt(i);
                var newCounter = new CounterContext(currentCounterDeclaration.CounterName,
                                                    new CounterRange(currentScopeDeclaration.Start.Value,
                                                                     currentScopeDeclaration.End.Value));
                this.counters.Add(newCounter);
            }
        }

        private void InitializeAllCounters()
        {
            foreach (var aCounter in Counters)
            {
                aCounter.Next();
            }
        }
    }
}
