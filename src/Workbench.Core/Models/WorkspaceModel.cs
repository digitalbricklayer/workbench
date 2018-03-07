using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Workspace model where the model, display and solution are stored.
    /// </summary>
    [Serializable]
    public class WorkspaceModel : AbstractModel
    {
        private ModelModel model;
        private SolutionModel solution;
        private DisplayModel display;

        /// <summary>
        /// Initialize a workspace with a model name.
        /// </summary>
        public WorkspaceModel(ModelName theModelName)
        {
            Contract.Requires<ArgumentNullException>(theModelName != null);

            Model = new ModelModel(theModelName);
            Solution = new SolutionModel(Model);
            Display = new DisplayModel(Model);
        }

        /// <summary>
        /// Initialize a workspace with default values.
        /// </summary>
        public WorkspaceModel()
        {
            Model = new ModelModel();
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
        /// Create a new workspace.
        /// </summary>
        /// <param name="theModelName">Name for the model.</param>
        /// <returns>Fluent interface context.</returns>
        public static WorkspaceContext Create(string theModelName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theModelName));
            var newWorkspace = new WorkspaceModel(new ModelName(theModelName));
            return new WorkspaceContext(newWorkspace);
        }

        /// <summary>
        /// Create a new workspace.
        /// </summary>
        /// <returns>Fluent interface context.</returns>
        public static WorkspaceContext Create()
        {
            return new WorkspaceContext(new WorkspaceModel());
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
        /// Get the graphic with the matching name.
        /// </summary>
        /// <param name="theName">Name of the graphic.</param>
        /// <returns>Graphic matching the name.</returns>
        public GraphicModel GetGraphicBy(string theName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));
            return Display.GetGraphicBy(theName);
        }

        /// <summary>
        /// Update the solution from a snapshot.
        /// </summary>
        /// <param name="theSolveResult">Solution snapshot.</param>
        public void UpdateSolutionFrom(SolveResult theSolveResult)
        {
            Contract.Requires<ArgumentNullException>(theSolveResult != null);
            Display.UpdateFrom(theSolveResult);
            Solution.UpdateSolutionFrom(theSolveResult);
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
