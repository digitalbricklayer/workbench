using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Solution display model responsible for displaying, or visualizing a solution.
    /// </summary>
    [Serializable]
    public class DisplayModel : AbstractModel
    {
        private VisualizerBindingExpressionModel binding;
        private ObservableCollection<VisualizerModel> visualizers;

        /// <summary>
        /// Initialize a display model with default values.
        /// </summary>
        public DisplayModel()
        {
            Visualizers = new ObservableCollection<VisualizerModel>();
            Binding = new VisualizerBindingExpressionModel();
        }

        /// <summary>
        /// Gets or sets the visualizer collection.
        /// </summary>
        public ObservableCollection<VisualizerModel> Visualizers
        {
            get { return this.visualizers; }
            set
            {
                this.visualizers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the visualizer binding expression.
        /// </summary>
        public VisualizerBindingExpressionModel Binding
        {
            get { return this.binding; }
            private set
            {
                this.binding = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Add a new visualizer.
        /// </summary>
        /// <param name="theVisualizer">New visualizer.</param>
        public void AddVisualizer(VisualizerModel theVisualizer)
        {
            Contract.Requires<ArgumentNullException>(theVisualizer != null);
            theVisualizer.AssignIdentity();
            Visualizers.Add(theVisualizer);
        }

        /// <summary>
        /// Get the visualizer bound by name.
        /// </summary>
        /// <param name="theName">Visualizers name.</param>
        /// <returns>Visualizers.</returns>
        public VisualizerModel GetVisualizerBy(string theName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));
            return (from aViewerViewModel in Visualizers
                    where aViewerViewModel.Name == theName
                    select aViewerViewModel).FirstOrDefault();
        }

        /// <summary>
        /// Update the solution from a snapshot.
        /// </summary>
        /// <param name="theSnapshot">Solution snapshot.</param>
        public void UpdateFrom(SolutionSnapshot theSnapshot)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            Binding.ExecuteWith(new VisualizerUpdateContext(theSnapshot, this));
        }
    }
}
