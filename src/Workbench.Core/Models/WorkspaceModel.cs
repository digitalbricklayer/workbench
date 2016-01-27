using System;

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
        /// Initialize a workspace with default values.
        /// </summary>
        public WorkspaceModel()
        {
            this.Model = new ModelModel();
            this.Solution = new SolutionModel();
            this.Display = new DisplayModel();
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
        /// Gets or sets the solution display.
        /// </summary>
        public DisplayModel Display
        {
            get { return display; }
            set
            {
                display = value;
                OnPropertyChanged();
            }
        }
    }
}
