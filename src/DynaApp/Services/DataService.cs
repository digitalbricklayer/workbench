using System;
using Dyna.Core.Models;

namespace DynaApp.Services
{
    /// <summary>
    /// Service for controlling access to the model.
    /// </summary>
    public class DataService
    {
        /// <summary>
        /// Initialize a data service with default values.
        /// </summary>
        public DataService()
        {
            this.IsModelDirty = false;
        }

        /// <summary>
        /// Gets whether the model is currently up-to-date on disk.
        /// </summary>
        public bool IsModelDirty { get; private set; }

        /// <summary>
        /// Gets the current workspace model.
        /// </summary>
        public WorkspaceModel CurrentWorkspace { get; private set; }

        /// <summary>
        /// Reset the data back to default state.
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// Get a new workspace.
        /// </summary>
        /// <returns>A new workspace.</returns>
        public WorkspaceModel GetWorkspace()
        {
            if (this.CurrentWorkspace == null)
                this.CurrentWorkspace = new WorkspaceModel();

            return this.CurrentWorkspace;
        }

        /// <summary>
        /// Get a model for the workspace.
        /// </summary>
        /// <returns>A new model.</returns>
        public ModelModel GetModelFor(WorkspaceModel theWorkspace)
        {
            return new ModelModel();
        }
    }
}
