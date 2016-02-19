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

        /// <summary>
        /// Initialize a workspace with default values.
        /// </summary>
        public WorkspaceModel()
        {
            this.Model = new ModelModel();
            this.Solution = new SolutionModel(this.Model);
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public ModelModel Model
        {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        public SolutionModel Solution
        {
            get { return solution; }
            set
            {
                solution = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Add the visualizer.
        /// </summary>
        /// <param name="theVisualizer">The visualizer to add.</param>
        public void AddVisualizer(VariableVisualizerModel theVisualizer)
        {
            Contract.Requires<ArgumentNullException>(theVisualizer != null);
            this.Solution.AddVisualizer(theVisualizer);
        }

        /// <summary>
        /// Create a new workspace.
        /// </summary>
        /// <param name="theModelName">Name for the model.</param>
        /// <returns>Fluent interface context.</returns>
        public static WorkspaceContext Create(string theModelName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theModelName));
            var newWorkspace = new WorkspaceModel();
            newWorkspace.Model.Name = theModelName;
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
        /// Update the solution from a snapshot.
        /// </summary>
        /// <param name="theSnapshot">The snapshot.</param>
        public void UpdateSolutionFrom(SolutionSnapshot theSnapshot)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            this.Solution.UpdateSolutionFrom(theSnapshot);
        }
    }
}
