using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Castle.Core.Internal;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Solution display model responsible for displaying, or visualizing a solution.
    /// </summary>
    [Serializable]
    public class DisplayModel : AbstractModel
    {
        private ObservableCollection<VisualizerModel> visualizers;
        private ObservableCollection<VisualizerBindingExpressionModel> bindingExpressions;

        /// <summary>
        /// Initialize a display model with default values.
        /// </summary>
        public DisplayModel()
        {
            Visualizers = new ObservableCollection<VisualizerModel>();
            Bindings = new ObservableCollection<VisualizerBindingExpressionModel>();
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
        /// Gets or sets the visualizer binding expression collection.
        /// </summary>
        public ObservableCollection<VisualizerBindingExpressionModel> Bindings
        {
            get { return this.bindingExpressions; }
            set
            {
                this.bindingExpressions = value;
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
        /// <param name="theName">Visualizer name.</param>
        /// <returns>Visualizer.</returns>
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
        /// <param name="theSnapshot">The solution snapshot.</param>
        public void UpdateFrom(SolutionSnapshot theSnapshot)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            Visualizers.ForEach(aVisualizer => aVisualizer.UpdateFrom(theSnapshot));
        }
    }
}
