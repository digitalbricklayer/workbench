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
        private List<VisualizerModel> visualizers;
        private readonly ModelModel model;

        /// <summary>
        /// Initialize a display model with default values.
        /// </summary>
        /// <param name="theModel"></param>
        public DisplayModel(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            Visualizers = new List<VisualizerModel>();
            this.bindings = new List<VisualizerBindingExpressionModel>();
            this.model = theModel;
        }

        /// <summary>
        /// Gets or sets the visualizer collection.
        /// </summary>
        public List<VisualizerModel> Visualizers
        {
            get => this.visualizers;
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
            get
            {
                return new ReadOnlyCollection<VisualizerBindingExpressionModel>(this.bindings);
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
        /// Get the visualizer by name.
        /// </summary>
        /// <param name="theName">Visualizer name.</param>
        /// <returns>Visualizer matching the unique name.</returns>
        public VisualizerModel GetVisualizerBy(string theName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));

            return (from aVisualizer in Visualizers
                    where aVisualizer.Name == theName
                    select aVisualizer).FirstOrDefault();
        }

        /// <summary>
        /// Update the solution from a snapshot.
        /// </summary>
        /// <param name="theResult">Solution snapshot.</param>
        public void UpdateFrom(SolveResult theResult)
        {
            Contract.Requires<ArgumentNullException>(theResult != null);
            this.bindings.ForEach(binding => binding.ExecuteWith(new VisualizerUpdateContext(theResult.Snapshot, this, binding, this.model)));
            Visualizers.ForEach(visualizer => visualizer.UpdateWith(new PropertyUpdateContext(theResult.Snapshot, this, this.model)));
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
