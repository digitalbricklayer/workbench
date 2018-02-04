using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A solution to a model.
    /// </summary>
    [Serializable]
    public class SolutionModel : AbstractModel
    {
        private DisplayModel display;
        private SolutionSnapshot snapshot;

        /// <summary>
        /// Initialize the solution with the model.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        public SolutionModel(ModelModel theModel, SolutionSnapshot theSnapshot)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theSnapshot != null);

            Model = theModel;
            Snapshot = theSnapshot;
            Display = new DisplayModel(Model);
            Snapshot = new SolutionSnapshot();
        }

        /// <summary>
        /// Initialize the solution with the model.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        public SolutionModel(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            Model = theModel;
            Snapshot = new SolutionSnapshot();
            Display = new DisplayModel(Model);
            Snapshot = new SolutionSnapshot();
        }

        /// <summary>
        /// Gets or sets the solution display.
        /// </summary>
        public DisplayModel Display
        {
            get { return this.display; }
            set
            {
                this.display = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the solution snapshot.
        /// </summary>
        public SolutionSnapshot Snapshot
        {
            get { return this.snapshot; }
            set
            {
                this.snapshot = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the model this solution solves.
        /// </summary>
        public ModelModel Model { get; private set; }

        /// <summary>
        /// Get the value matching the name.
        /// </summary>
        /// <param name="theVariableName">Text of the variable to find.</param>
        /// <returns>Value matching the name. Null if no value matches the name.</returns>
        public ValueModel GetSingletonVariableValueByName(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return Snapshot.GetSingletonVariableValueByName(theVariableName);
        }

        /// <summary>
        /// Get the aggregate value matching the name.
        /// </summary>
        /// <param name="theVariableName">Aggregate value.</param>
        /// <returns>Aggregate value matching the name. Null if no aggregates matche the name.</returns>
        public ValueModel GetAggregateVariableValueByName(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return Snapshot.GetAggregateVariableValueByName(theVariableName);
        }

        /// <summary>
        /// Add the visualizer.
        /// </summary>
        /// <param name="theVisualizer">The visualizer to add.</param>
        public void AddVisualizer(VisualizerModel theVisualizer)
        {
            Contract.Requires<ArgumentNullException>(theVisualizer != null);
            Display.AddVisualizer(theVisualizer);
        }

        /// <summary>
        /// Get the visualizer with the matching name.
        /// </summary>
        /// <param name="theName">Text of the visualizer.</param>
        /// <returns>Visualizer matching the name.</returns>
        public VisualizerModel GetVisualizerBy(string theName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));
            return Display.GetVisualizerBy(theName);
        }

        /// <summary>
        /// Update the solution from a snapshot.
        /// </summary>
        /// <param name="theSolveResult">Solution snapshot.</param>
        public void UpdateSolutionFrom(SolveResult theSolveResult)
        {
            Contract.Requires<ArgumentNullException>(theSolveResult != null);
            Display.UpdateFrom(theSolveResult);
            Snapshot = theSolveResult.Snapshot;
        }

        /// <summary>
        /// Add a new visualizer binding expression.
        /// </summary>
        /// <param name="newBindingExpression">New visualizer binding expression.</param>
        public void AddBindingExpression(VisualizerBindingExpressionModel newBindingExpression)
        {
            Contract.Requires<ArgumentNullException>(newBindingExpression != null);
            Display.AddBindingEpxression(newBindingExpression);
        }
    }
}
