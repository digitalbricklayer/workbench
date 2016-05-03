using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solver
{
    internal class CounterRange
    {
        public CounterRange(int low, int high)
        {
            Contract.Requires<ArgumentException>(high >= low);
            Low = low;
            High = high;
        }

        public int Low { get; private set; }
        public int High { get; private set; }
    }
}