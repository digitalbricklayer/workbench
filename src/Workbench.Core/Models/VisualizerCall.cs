using System.Collections.Generic;

namespace Workbench.Core.Models
{
    public class VisualizerCall
    {
        public VisualizerCall()
        {
            Arguments = new List<CallArgument>();
        }

        public IReadOnlyCollection<CallArgument> Arguments { get; private set; }
    }
}