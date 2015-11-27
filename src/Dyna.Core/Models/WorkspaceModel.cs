using System;

namespace Dyna.Core.Models
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
    }
}
