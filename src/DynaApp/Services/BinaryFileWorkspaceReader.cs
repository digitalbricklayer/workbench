using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Dyna.Core.Models;

namespace DynaApp.Services
{
    /// <summary>
    /// Workspace model reader.
    /// </summary>
    public class BinaryFileWorkspaceReader : IWorkspaceReader
    {
        /// <summary>
        /// Read a workspace model from a file.
        /// </summary>
        /// <returns>Workspace model.</returns>
        public WorkspaceModel Read(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("filename");
            using (var fileStream = File.OpenRead(filename))
            {
                var binaryFormatter = new BinaryFormatter();
                return (WorkspaceModel) binaryFormatter.Deserialize(fileStream);
            }
        }
    }
}
