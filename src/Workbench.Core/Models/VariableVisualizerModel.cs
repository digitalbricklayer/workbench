using System;
using System.Windows;

namespace Workbench.Core.Models
{
    [Serializable]
    public class VariableVisualizerModel : GraphicModel
    {
        public VariableVisualizerModel(Point newLocation)
            : base("A visualizer", newLocation)
        {
        }
    }
}
