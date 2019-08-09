using System;

namespace Workbench.Core.Models
{
    public class VisualizerUpdateContext
    {
        public VisualizerUpdateContext(SolutionSnapshot theSnapshot, DisplayModel theDisplay, VisualizerBindingExpressionModel theBinding, ModelModel theModel)
        {
            Snapshot = theSnapshot;
            Display = theDisplay;
            Binding = theBinding;
            Model = theModel;
        }

        public SolutionSnapshot Snapshot { get; }
        public DisplayModel Display { get; }
        public VisualizerBindingExpressionModel Binding { get; }
        public ModelModel Model { get; }
    }
}
