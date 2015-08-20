using System;

namespace DynaApp.Models
{
    /// <summary>
    /// Workspace model where the model and solution are stored.
    /// </summary>
    [Serializable]
    public class WorkspaceModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public ModelModel Model { get; set; }

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        public SolutionModel Solution { get; set; }
    }
}
