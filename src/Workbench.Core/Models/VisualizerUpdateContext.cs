using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    public class VisualizerUpdateContext
    {
        public VisualizerUpdateContext(SolutionSnapshot theSnapshot, DisplayModel theDisplay, VisualizerBindingExpressionModel theBinding)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            Contract.Requires<ArgumentNullException>(theDisplay != null);
            Contract.Requires<ArgumentNullException>(theBinding != null);

            Snapshot = theSnapshot;
            Display = theDisplay;
            Binding = theBinding;
        }

        public SolutionSnapshot Snapshot { get; }
        public DisplayModel Display { get; }
        public VisualizerBindingExpressionModel Binding { get; }
    }
}
