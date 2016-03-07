using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Workbench.Core.Models;

namespace Workbench.Services
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
            using (var fileStream = File.OpenRead(filename))
            {
                var binaryFormatter = new BinaryFormatter();
                return (WorkspaceModel) binaryFormatter.Deserialize(fileStream);
            }
        }
    }
}
