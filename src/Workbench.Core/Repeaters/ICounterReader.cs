using System.Collections.Generic;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
{
    internal interface ICounterReader
    {
        /// <summary>
        /// Read the counters from the repeater statement list.
        /// </summary>
        /// <returns>List of counters read from the source.</returns>
        IList<CounterContext> ReadFrom(MultiRepeaterStatementNode theRepeaterStatementList);
    }
}
