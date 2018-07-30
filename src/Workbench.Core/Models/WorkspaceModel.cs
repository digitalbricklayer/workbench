using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Workspace model where the model and solution are stored.
    /// </summary>
    [Serializable]
    public class WorkspaceModel : AbstractModel
    {
        private ModelModel model;
        private SolutionModel solution;

        /// <summary>
        /// Initialize a workspace with a model name.
        /// </summary>
        public WorkspaceModel(ModelName theModelName)
        {
            Contract.Requires<ArgumentNullException>(theModelName != null);

            Model = new ModelModel(theModelName);
            Solution = new SolutionModel(Model);
        }

        /// <summary>
        /// Initialize a workspace with default values.
        /// </summary>
        public WorkspaceModel()
        {
            Model = new ModelModel();
            Solution = new SolutionModel(Model);
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
        }

        /// <summary>
        /// Update the solution from a snapshot.
        /// </summary>
        /// <param name="theSolveResult">Solution snapshot.</param>
        public void UpdateSolutionFrom(SolveResult theSolveResult)
        {
            Contract.Requires<ArgumentNullException>(theSolveResult != null);
            Solution.UpdateSolutionFrom(theSolveResult);
        }

        /// <summary>
        /// Add a new visualizer binding expression.
        /// </summary>
        /// <param name="newBindingExpression">New visualizer binding expression.</param>
        public void AddBindingExpression(VisualizerBindingExpressionModel newBindingExpression)
        {
            Contract.Requires<ArgumentNullException>(newBindingExpression != null);
        }

        public VisualizerModel GetTabBy(string theTabName)
        {
            throw new NotImplementedException();
        }
    }
}
