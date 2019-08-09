using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
{
    internal class OrConstraintRepeaterContext
    {
        private readonly IList<CounterContext> counters;
        private bool firstIterationPassed;

        /// <summary>
        /// Initialize a repeater context from the constraint.
        /// </summary>
        /// <param name="theConstraint">Expression constraint.</param>
        /// <param name="theModel">Model</param>
        public OrConstraintRepeaterContext(ExpressionConstraintModel theConstraint, ModelModel theModel)
        {
            Constraint = theConstraint;
            Model = theModel;
            this.counters = new List<CounterContext>();
            if (!theConstraint.Expression.Node.HasExpander) return;
            CreateCounterContextsFrom(theConstraint.Expression.Node.Expander);
        }

        /// <summary>
        /// Gets the counters.
        /// </summary>
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
        /// Gets the model.
        /// </summary>
        public ModelModel Model { get; private set; }

        /// <summary>
        /// Move to the next counter value set.
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

            for (var i = this.counters.Count - 1; i >= 0; i--)
            {
                var currentCounterStatus = this.counters[i].Next();
                if (currentCounterStatus)
                {
                    // Found counter with remaining value, reset the rightmost counters...
                    for (var j = i+1; j  < this.counters.Count; j++)
                    {
                        this.counters[j].Reset();
                        this.counters[j].Next();
                    }

                    return true;
                }

                // Try the next counter...
                continue;
            }

            return false;
        }

        /// <summary>
        /// Get the counter context matching the counter name.
        /// </summary>
        /// <param name="counterName">Counter name.</param>
        /// <returns>Counter context with the counter name.</returns>
        public CounterContext GetCounterContextByName(string counterName)
        {
            return Counters.First(_ => _.CounterName == counterName);
        }

        private void CreateCounterContextsFrom(MultiRepeaterStatementNode theRepeater)
        {
            for (var i = 0; i < theRepeater.CounterDeclarations.CounterDeclarations.Count; i++)
            {
                var currentCounterDeclaration = theRepeater.CounterDeclarations.CounterDeclarations.ElementAt(i);
                var currentScopeDeclaration = theRepeater.ScopeDeclarations.ScopeDeclarations.ElementAt(i);
                CounterContext newCounter;
                if (currentScopeDeclaration.IsCount)
                {
                    newCounter = new CounterContext(currentCounterDeclaration.CounterName,
                                                    new CountIterator(CreateValueSourceFrom(currentScopeDeclaration.Count)));
                }
                else if (currentScopeDeclaration.IsScope)
                {
                    newCounter = new CounterContext(currentCounterDeclaration.CounterName,
                                                    new RangeIterator(CreateValueSourceFrom(currentScopeDeclaration.Scope.Start),
                                                                      CreateValueSourceFrom(currentScopeDeclaration.Scope.End)));
                }
                else
                {
                    throw new NotImplementedException();
                }
                this.counters.Add(newCounter);
            }
        }

        private void InitializeAllCounters()
        {
            for (var i = 0; i < this.counters.Count; i++)
            {
                var currentSuccess = this.counters[i].Next();
                if (currentSuccess)
                {
                    // Success, initialize the next counter...
                    continue;
                }

                // Failure, go backwards until a counter is successfully initialized
                if (i == 0) return;

                // Move the immediately leftward value on one and then reset the current counter...
                this.counters[i-1].Next();
                this.counters[i].Reset();
                this.counters[i].Next();
            }
        }

        private ILimitValueSource CreateValueSourceFrom(ScopeLimitSatementNode limitNode)
        {
            Debug.Assert(limitNode.IsLiteral || limitNode.IsCounterReference);

            if (limitNode.IsLiteral) return new LiteralLimitValueSource(limitNode.Literal);
            var theCounter = Counters.First(counter => counter.CounterName == limitNode.CounterReference.CounterName);
            return new CounterLimitValueSource(theCounter);
        }

        private ILimitValueSource CreateValueSourceFrom(ExpanderCountNode countNode)
        {
            Debug.Assert(countNode.IsLiteral || countNode.IsCounterReference || countNode.IsFunctionInvocation);

            switch (countNode.InnerExpression)
            {
                case IntegerLiteralNode literalNode:
                    return new LiteralLimitValueSource(literalNode);

                case FunctionInvocationNode functionInvocationNode:
                    var functionInvocationContext = new FunctionInvocationContext(functionInvocationNode, Model);
                    return new FunctionInvocationValueSource(functionInvocationContext);

                case CounterReferenceNode counterReferenceNode:
                    var counterReference = Counters.First(counter => counter.CounterName == counterReferenceNode.CounterName);
                    return new CounterLimitValueSource(counterReference);

                default:
                    throw new NotImplementedException("Unknown count expression.");
            }
        }
    }
}
