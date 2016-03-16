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
        private ObservableCollection<VisualizerModel> visualizers;

        /// <summary>
        /// Initialize a display model with default values.
        /// </summary>
        public DisplayModel()
        {
            this.Visualizers = new ObservableCollection<VisualizerModel>();
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
        /// Add a new visualizer.
        /// </summary>
        /// <param name="theVisualizer">New visualizer.</param>
        public void AddVisualizer(VisualizerModel theVisualizer)
        {
            Contract.Requires<ArgumentNullException>(theVisualizer != null);
            theVisualizer.AssignIdentity();
            this.Visualizers.Add(theVisualizer);
        }

        /// <summary>
        /// Get the visualizer bound to the variable matching the variable name.
        /// </summary>
        /// <param name="theVariableName">Name of the variable.</param>
        /// <returns>Visualizer bound to the variable matching the variable name.</returns>
        public VisualizerModel GetVisualizerFor(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return (from aViewerViewModel in this.Visualizers
                    where aViewerViewModel.Binding.Name == theVariableName
                    select aViewerViewModel).FirstOrDefault();
        }

        /// <summary>
        /// Update the solution from a snapshot.
        /// </summary>
        /// <param name="theSnapshot">The solution snapshot.</param>
        public void UpdateSolutionFrom(SolutionSnapshot theSnapshot)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            foreach (var aVisualizer in this.Visualizers)
            {
                if (!aVisualizer.Binding.HasBinding) continue;
                var theSingletonValue = theSnapshot.GetSingletonVariableValueByName(aVisualizer.Binding.Name);
                if (theSingletonValue != null)
                    aVisualizer.Value = theSingletonValue;
                else
                {
                    var theAggregateValue = theSnapshot.GetAggregateVariableValueByName(aVisualizer.Binding.Name);
                    if (theAggregateValue != null)
                        aVisualizer.Value = theAggregateValue;
                }
            }
        }
    }
}
