using System.IO;
using Dyna.Core.Models;
using NUnit.Framework;

namespace Dyna.UI.Tests.Integration.Models
{
    [TestFixture]
    public class WorkspaceWriterTests
    {
        [Test]
        public void Write_A_Model_Then_Read_Back_Same_Model()
        {
            var orginalWorkspaceModel = WorkspaceModelFactory.Create();
            var filePath = Path.GetTempFileName();
            WriteWorkspaceToDisk(filePath, orginalWorkspaceModel);
            var readWorkspaceModel = ReadWorkspaceModel(filePath);
            Assert.That(readWorkspaceModel, Is.Not.Null);
            File.Delete(filePath);
        }

        private static WorkspaceModel ReadWorkspaceModel(string filePath)
        {
            var workspaceReader = new WorkspaceReader(filePath);
            return workspaceReader.Read();
        }

        private static void WriteWorkspaceToDisk(string filePath, WorkspaceModel workspaceModel)
        {
            var workspaceWriter = new WorkspaceWriter(filePath);
            workspaceWriter.Write(workspaceModel);
        }
    }
}
