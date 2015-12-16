using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Dyna.Core.Models;

namespace DynaApp.Services
{
    /// <summary>
    /// Workspace model writer.
    /// </summary>
    public class BinaryFileWorkspaceWriter : IWorkspaceWriter
    {
        /// <summary>
        /// Write a workspace model to a file.
        /// </summary>
        /// <param name="filename">File path.</param>
        /// <param name="theWorkspace">Workspace model.</param>
        public void Write(string filename, WorkspaceModel theWorkspace)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("filename");
            if (theWorkspace == null)
                throw new ArgumentNullException("theWorkspace");

            using (var fileStream = File.OpenWrite(filename))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, theWorkspace);
            }
        }
    }
}
