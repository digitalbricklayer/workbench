using System;
using System.Collections.ObjectModel;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Solution display model responsible for displaying, or visualizing a solution.
    /// </summary>
    [Serializable]
    public class DisplayModel : AbstractModel
    {
        private ObservableCollection<VariableVisualizerModel> visualizers;

        /// <summary>
        /// Initialize a display model with default values.
        /// </summary>
        public DisplayModel()
        {
            this.Visualizers = new ObservableCollection<VariableVisualizerModel>();
        }

        /// <summary>
        /// Gets or sets the visualizer collection.
        /// </summary>
        public ObservableCollection<VariableVisualizerModel> Visualizers
        {
            get { return this.visualizers; }
            set
            {
                this.visualizers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Add a new visualizer.
        /// </summary>
        /// <param name="theVisualizer">New visualizer.</param>
        public void AddVisualizer(VariableVisualizerModel theVisualizer)
        {
            if (theVisualizer == null)
                throw new ArgumentNullException("theVisualizer");
            this.Visualizers.Add(theVisualizer);
        }
    }
}
