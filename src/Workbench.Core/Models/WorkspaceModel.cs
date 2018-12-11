using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// The workspace contains all of the model and a solution if the model has been solved successfully.
    /// </summary>
    [Serializable]
    public class WorkspaceModel : AbstractModel
    {
        private ModelModel model;
        private SolutionModel solution;
        private DisplayModel display;

        /// <summary>
        /// Initialize a workspace with a model.
        /// </summary>
        public WorkspaceModel(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            Model = theModel;
            Model.Workspace = this;
            Solution = new SolutionModel(Model);
            Display = new DisplayModel(Model);
        }

        /// <summary>
        /// Initialize a workspace with a model name.
        /// </summary>
        public WorkspaceModel(ModelName theModelName)
        {
            Contract.Requires<ArgumentNullException>(theModelName != null);

            Model = new ModelModel(theModelName);
            Model.Workspace = this;
            Solution = new SolutionModel(Model);
            Display = new DisplayModel(Model);
        }

        /// <summary>
        /// Initialize a workspace with default values.
        /// </summary>
        public WorkspaceModel()
        {
            Model = new ModelModel();
            Model.Workspace = this;
            Solution = new SolutionModel(Model);
            Display = new DisplayModel(Model);
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public ModelModel Model
        {
            get { return this.model; }
            set
            {
                this.model = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        public SolutionModel Solution
        {
            get { return this.solution; }
            set
            {
                this.solution = value; 
                OnPropertyChanged();
            }
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
        /// Solve the model.
        /// </summary>
        /// <returns>Solve result.</returns>
        public SolveResult Solve()
        {
            Contract.Ensures(Contract.Result<SolveResult>() != null);
            Contract.Assume(Model != null);
            var solveResult = Model.Solve();
            if (solveResult.IsFailure) return solveResult;
            UpdateSolutionFrom(solveResult);

            return solveResult;
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
        /// Update the solution from a solve result.
        /// </summary>
        /// <param name="theSolveResult">Solve result.</param>
        public void UpdateSolutionFrom(SolveResult theSolveResult)
        {
            Contract.Requires<ArgumentNullException>(theSolveResult != null);
            Display.UpdateFrom(theSolveResult);
            Solution.UpdateFrom(theSolveResult);
        }

        /// <summary>
        /// Add a new visualizer binding expression.
        /// </summary>
        /// <param name="newBindingExpression">New visualizer binding expression.</param>
        public void AddBindingExpression(VisualizerBindingExpressionModel newBindingExpression)
        {
            Contract.Requires<ArgumentNullException>(newBindingExpression != null);
            Display.AddBindingExpression(newBindingExpression);
        }

        /// <summary>
        /// Delete the visualizer binding from the workspace.
        /// </summary>
        /// <param name="aVisualizerBinding">Visualizer binding to delete.</param>
        public void DeleteBindingExpression(VisualizerBindingExpressionModel aVisualizerBinding)
        {
            Contract.Requires<ArgumentNullException>(aVisualizerBinding != null);
            Display.DeleteBindingExpression(aVisualizerBinding);
        }

        /// <summary>
        /// Get the visualizer with the matching name.
        /// </summary>
        /// <param name="theName">Name of the visualizer.</param>
        /// <returns>Visualizer matching the name.</returns>
        public VisualizerModel GetVisualizerBy(string theName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));
            return Display.GetVisualizerBy(theName);
        }
    }
}
