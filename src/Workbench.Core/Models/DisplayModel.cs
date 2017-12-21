using System;
using System.Collections.Generic;
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
        private readonly List<VisualizerBindingExpressionModel> bindings;
        private ObservableCollection<VisualizerModel> visualizers;
        private readonly ModelModel model;

        /// <summary>
        /// Initialize a display model with default values.
        /// </summary>
        /// <param name="theModel"></param>
        public DisplayModel(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            Visualizers = new ObservableCollection<VisualizerModel>();
            this.bindings = new List<VisualizerBindingExpressionModel>();
            this.model = theModel;
        }

        /// <summary>
        /// Gets or sets the visualizer collection.
        /// </summary>
        public ObservableCollection<VisualizerModel> Visualizers
        {
            get { return this.visualizers; }
            private set
            {
				Contract.Requires<ArgumentNullException>(value != null);

                this.visualizers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the visualizer binding expression.
        /// </summary>
        public IReadOnlyCollection<VisualizerBindingExpressionModel> Bindings
        {
            get { return new ReadOnlyCollection<VisualizerBindingExpressionModel>(this.bindings); }
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
        /// Get the visualizer by name.
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
        public void UpdateFrom(SolveResult theSnapshot)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            this.bindings.ForEach(binding => binding.ExecuteWith(new VisualizerUpdateContext(theSnapshot.Snapshot, this, binding, this.model)));
        }

        /// <summary>
        /// Add a new visualizer binding expression.
        /// </summary>
        /// <param name="newBindingExpression">New visualizer binding expression.</param>
        public void AddBindingEpxression(VisualizerBindingExpressionModel newBindingExpression)
        {
            Contract.Requires<ArgumentNullException>(newBindingExpression != null);
            newBindingExpression.AssignIdentity();
            this.bindings.Add(newBindingExpression);
        }

        /// <summary>
        /// Get the visualizer binding expression matching the id.
        /// </summary>
        /// <param name="id">Id of the visualizer to be found.</param>
        /// <returns>Visualizer binding expression matching the id or null if not found.</returns>
        public VisualizerBindingExpressionModel GetVisualizerBindingById(int id)
        {
            return Bindings.FirstOrDefault(binding => binding.Id == id);
        }
    }
}
