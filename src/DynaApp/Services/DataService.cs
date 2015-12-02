using System;
using Dyna.Core.Models;

namespace DynaApp.Services
{
    /// <summary>
    /// Service for controlling access to the data.
    /// </summary>
    public class DataService
    {
        /// <summary>
        /// Gets whether the model is currently up-to-date on disk.
        /// </summary>
        public bool IsModelDirty { get; private set; }

        /// <summary>
        /// Save the data to a storage medium.
        /// </summary>
        public SaveResult Save(string file, WorkspaceModel theWorkspace)
        {
            try
            {
                var workspaceWriter = new WorkspaceModelWriter(file);
                workspaceWriter.Write(theWorkspace);

                return SaveResult.Success;
            }
            catch (Exception e)
            {
                return new SaveResult(SaveStatus.Failure, e.Message);
            }
        }

        /// <summary>
        /// Open the storgage medium and read the contents.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        public void Open(string file)
        {
            try
            {
                // Load file
                var workspaceReader = new WorkspaceModelReader(file);
                var theWorkspaceModel = workspaceReader.Read();
            }
            catch (Exception e)
            {
                return;
            }
        }

        /// <summary>
        /// Reset the data back to default state.
        /// </summary>
        public void Reset()
        {
        }
    }

    public class SaveResult
    {
        public SaveResult(SaveStatus theStatus, string theMessage = null)
        {
            this.Message = string.Empty;
            this.Status = theStatus;
            if (theMessage != null)
                this.Message = theMessage;
        }

        public SaveStatus Status { get; private set; }
        public string Message { get; private set; }

        public static SaveResult Failure
        {
            get
            {
                return new SaveResult(SaveStatus.Failure);
            }
        }

        public static SaveResult Success
        {
            get
            {
                return new SaveResult(SaveStatus.Success);
            }
        }
    }

    public enum SaveStatus
    {
        Failure,
        Success
    }
}
