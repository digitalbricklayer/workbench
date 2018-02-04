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
        private readonly VisualizerUpdateContext context;

        public VisualizerRepeaterContext(VisualizerUpdateContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theContext != null);
            this.counters = new List<CounterContext>();
            this.context = theContext;
            Model = theContext.Model;
            if (!Binding.Node.HasExpander) return;
            CreateCounterContextsFrom(Binding.Node.Expander);
        }

        public DisplayModel Display => this.context.Display;

        public VisualizerBindingExpressionModel Binding => this.context.Binding;

        public bool HasRepeaters => this.counters.Any();

        public IReadOnlyCollection<CounterContext> Counters => new ReadOnlyCollection<CounterContext>(counters);

        /// <summary>
        /// Gets the model.
        /// </summary>
        public ModelModel Model { get; private set; }

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

        /// <summary>
        /// Get a visualizer using the visualizer name.
        /// </summary>
        /// <param name="vizualizerName">Text of the visualizer.</param>
        /// <returns>Visualizer matching the name.</returns>
        public VisualizerModel GetVisualizerByName(string vizualizerName)
        {
            return Display.GetVisualizerBy(vizualizerName);
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
            Contract.Requires<ArgumentException>(limitNode.IsLiteral || limitNode.IsCounterReference || limitNode.IsFunctionInvocation);

            if (limitNode.IsLiteral) return new LiteralLimitValueSource(limitNode.Literal);
            if (limitNode.IsFunctionInvocation)
            {
                var functionInvocationContext = new FunctionInvocationContext(limitNode.FunctionInvocation, Model);
                return new FunctionInvocationValueSource(functionInvocationContext);
            }
            var theCounter = Counters.First(counter => counter.CounterName == limitNode.CounterReference.CounterName);
            return new CounterLimitValueSource(theCounter);
        }

        private ILimitValueSource CreateValueSourceFrom(ExpanderCountNode countNode)
        {
            Contract.Requires<ArgumentNullException>(countNode != null);
            Contract.Requires<ArgumentException>(countNode.IsLiteral || countNode.IsCounterReference || countNode.IsFunctionInvocation);

            if (countNode.IsLiteral) return new LiteralLimitValueSource(countNode.Literal);
            if (countNode.IsFunctionInvocation)
            {
                var functionInvocationContext = new FunctionInvocationContext(countNode.FunctionInvocation, Model);
                return new FunctionInvocationValueSource(functionInvocationContext);
            }
            var theCounter = Counters.First(counter => counter.CounterName == countNode.CounterReference.CounterName);
            return new CounterLimitValueSource(theCounter);
        }
    }
}
