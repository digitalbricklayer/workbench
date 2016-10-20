using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    public class VisualizerUpdateContext
    {
        public VisualizerUpdateContext(SolutionSnapshot theSnapshot, DisplayModel theDisplay)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            Contract.Requires<ArgumentNullException>(theDisplay != null);

            Snapshot = theSnapshot;
            Display = theDisplay;
        }

        public SolutionSnapshot Snapshot { get; }
        public DisplayModel Display { get; }
    }
}
