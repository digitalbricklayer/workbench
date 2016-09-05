using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
{
    internal class CounterReader : ICounterReader
    {
        private readonly IList<CounterContext> counters = new List<CounterContext>();

        /// <summary>
        /// Read the counters from the repeater statement list.
        /// </summary>
        /// <returns>List of counters read from the source.</returns>
        public IList<CounterContext> ReadFrom(MultiRepeaterStatementNode theRepeaterStatementList)
        {
            for (var i = 0; i < theRepeaterStatementList.CounterDeclarations.CounterDeclarations.Count; i++)
            {
                var currentCounterDeclaration = theRepeaterStatementList.CounterDeclarations.CounterDeclarations.ElementAt(i);
                var currentScopeDeclaration = theRepeaterStatementList.ScopeDeclarations.ScopeDeclarations.ElementAt(i);
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

            return this.counters;
        }

        private ILimitValueSource CreateValueSourceFrom(ScopeLimitSatementNode limitNode)
        {
            Contract.Requires<ArgumentNullException>(limitNode != null);
            Contract.Requires<ArgumentException>(limitNode.IsLiteral || limitNode.IsCounterReference);

            if (limitNode.IsLiteral) return new LiteralLimitValueSource(limitNode.Literal);
            var x = this.counters.First(counter => counter.CounterName == limitNode.CounterReference.CounterName);
            return new CounterLimitValueSource(x);
        }

        private ILimitValueSource CreateValueSourceFrom(ExpanderCountNode countNode)
        {
            Contract.Requires<ArgumentNullException>(countNode != null);
            Contract.Requires<ArgumentException>(countNode.IsLiteral || countNode.IsCounterReference);

            if (countNode.IsLiteral) return new LiteralLimitValueSource(countNode.Literal);
            var x = this.counters.First(counter => counter.CounterName == countNode.CounterReference.CounterName);
            return new CounterLimitValueSource(x);
        }
    }
}
