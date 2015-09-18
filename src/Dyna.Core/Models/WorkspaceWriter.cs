using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dyna.Core.Models
{
    /// <summary>
    /// Workspace model writer.
    /// </summary>
    public class WorkspaceWriter
    {
        private readonly string filename;

        /// <summary>
        /// Initialize worapce writer with file name.
        /// </summary>
        /// <param name="filename">File path for the output file.</param>
        public WorkspaceWriter(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("filename");
            this.filename = filename;
        }

        /// <summary>
        /// Write a workspace model to a file.
        /// </summary>
        /// <param name="theWorkspace">Workspace model.</param>
        public void Write(WorkspaceModel theWorkspace)
        {
            if (theWorkspace == null)
                throw new ArgumentNullException("theWorkspace");

            using (var fileStream = File.OpenWrite(this.filename))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, theWorkspace);
            }
        }
    }
}
