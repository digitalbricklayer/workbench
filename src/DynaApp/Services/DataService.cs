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
            this.FileName = string.Empty;
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
        /// Gets or sets the file path.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Save the data to a file.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        public SaveResult Save(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("file");

            this.FileName = file;

            try
            {
                var workspaceWriter = new WorkspaceModelWriter();
                workspaceWriter.Write(file, this.CurrentWorkspace);
            }
            catch (Exception e)
            {
                return new SaveResult(SaveStatus.Failure, e.Message);
            }

            return SaveResult.Success;
        }

        /// <summary>
        /// Open the file and read the contents.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        public OpenResult Open(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("file");

            this.FileName = file;

            try
            {
                // Load file
                var workspaceReader = new WorkspaceModelReader();
                this.CurrentWorkspace = workspaceReader.Read(file);
            }
            catch (Exception e)
            {
                return new OpenResult(OpenStatus.Failure, e.Message);
            }

            return OpenResult.Success;
        }

        /// <summary>
        /// Reset the data back to default state.
        /// </summary>
        public void Reset()
        {
            this.FileName = string.Empty;
        }

        /// <summary>
        /// Get a new workspace.
        /// </summary>
        /// <returns>A new workspace.</returns>
        public WorkspaceModel GetWorkspace()
        {
            var newWorkspace = new WorkspaceModel();
            this.CurrentWorkspace = newWorkspace;

            return newWorkspace;
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
