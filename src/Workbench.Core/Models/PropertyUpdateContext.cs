using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    public class PropertyUpdateContext
    {
        public PropertyUpdateContext(SolutionSnapshot theSnapshot, DisplayModel theDisplay, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            Contract.Requires<ArgumentNullException>(theDisplay != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            Snapshot = theSnapshot;
            Display = theDisplay;
            Model = theModel;
        }

        public SolutionSnapshot Snapshot { get; }
        public DisplayModel Display { get; }
        public ModelModel Model { get; }
    }
}