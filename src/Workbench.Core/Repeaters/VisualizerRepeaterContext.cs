using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
{
    internal class VisualizerRepeaterContext
    {
        private readonly IList<CounterContext> counters;
        private bool firstIterationPassed;
        private readonly ChessboardVisualizerModel visualizer;

        public VisualizerRepeaterContext(ChessboardVisualizerModel theVisualizer)
        {
            Contract.Requires<ArgumentNullException>(theVisualizer != null);
            this.counters = new List<CounterContext>();
            this.visualizer = theVisualizer;
            if (!theVisualizer.Binding.Node.HasExpander) return;
            CreateCounterContextsFrom(theVisualizer.Binding.Node.Expander);
        }

        public ChessboardVisualizerModel Visualizer => this.visualizer;

        public VisualizerBindingExpressionModel Binding => this.visualizer.Binding;

        public bool HasRepeaters => this.counters.Any();

        public IReadOnlyCollection<CounterContext> Counters => new ReadOnlyCollection<CounterContext>(counters);

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
                    for (var j = i + 1; j < this.counters.Count; j++)
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
                this.counters[i - 1].Next();
                this.counters[i].Reset();
                this.counters[i].Next();
            }
        }

        private ILimitValueSource CreateValueSourceFrom(ScopeLimitSatementNode limitNode)
        {
            Contract.Requires<ArgumentNullException>(limitNode != null);
            Contract.Requires<ArgumentException>(limitNode.IsLiteral || limitNode.IsCounterReference);

            if (limitNode.IsLiteral) return new LiteralLimitValueSource(limitNode.Literal);
            var x = Counters.First(_ => _.CounterName == limitNode.CounterReference.CounterName);
            return new CounterLimitValueSource(x);
        }

        private ILimitValueSource CreateValueSourceFrom(ExpanderCountNode countNode)
        {
            Contract.Requires<ArgumentNullException>(countNode != null);
            Contract.Requires<ArgumentException>(countNode.IsLiteral || countNode.IsCounterReference);

            if (countNode.IsLiteral) return new LiteralLimitValueSource(countNode.Literal);
            var x = Counters.First(_ => _.CounterName == countNode.CounterReference.CounterName);
            return new CounterLimitValueSource(x);
        }
    }
}
