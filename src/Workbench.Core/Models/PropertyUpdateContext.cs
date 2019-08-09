using System;

namespace Workbench.Core.Models
{
    public class PropertyUpdateContext
    {
        public PropertyUpdateContext(SolutionSnapshot theSnapshot, DisplayModel theDisplay, ModelModel theModel)
        {
            Snapshot = theSnapshot;
            Display = theDisplay;
            Model = theModel;
        }

        public SolutionSnapshot Snapshot { get; }
        public DisplayModel Display { get; }
        public ModelModel Model { get; }
    }
}