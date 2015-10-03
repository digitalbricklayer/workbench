using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dyna.Core.Models
{
    /// <summary>
    /// Workspace model reader.
    /// </summary>
    public class WorkspaceReader
    {
        private readonly string filename;

        /// <summary>
        /// Initialize worapce writer with file name.
        /// </summary>
        /// <param name="filename">File path for the output file.</param>
        public WorkspaceReader(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("filename");
            this.filename = filename;
        }

        /// <summary>
        /// Read a workspace model from a file.
        /// </summary>
        /// <returns>Workspace model.</returns>
        public WorkspaceModel Read()
        {
            using (var fileStream = File.OpenRead(this.filename))
            {
                var binaryFormatter = new BinaryFormatter();
                return (WorkspaceModel) binaryFormatter.Deserialize(fileStream);
            }
        }
    }
}
