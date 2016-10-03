using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    public class VisualizerUpdateContext
    {
        public VisualizerUpdateContext(SolutionSnapshot theSnapshot, VisualizerModel theVisualizer)
        {
            Snapshot = theSnapshot;
            Visualizer = theVisualizer;
        }

        public SolutionSnapshot Snapshot { get; }
        public VisualizerModel Visualizer { get; }
    }
}