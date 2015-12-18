using System.IO;
using Workbench.Services;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.UI.Tests.Integration.Services
{
    [TestFixture]
    public class BinaryFileWorkspaceWriterTests
    {
        [Test]
        public void WriteWorkspaceToDiskThenReadBackContainsSameWorkspace()
        {
            var filePath = Path.GetTempFileName();
            WriteWorkspaceToDisk(filePath);
            var readWorkspaceModel = ReadWorkspaceModel(filePath);
            Assert.That(readWorkspaceModel, Is.Not.Null);
            Assert.That(readWorkspaceModel.Model.Name, Is.Empty);
            var variableX = readWorkspaceModel.Model.GetVariableByName("x");
            Assert.That(variableX.Name, Is.EqualTo("x"));
            File.Delete(filePath);
        }

        private static WorkspaceModel ReadWorkspaceModel(string filePath)
        {
            var workspaceReader = new BinaryFileWorkspaceReader();
            return workspaceReader.Read(filePath);
        }

        private static void WriteWorkspaceToDisk(string filePath)
        {
            var workspaceModel = WorkspaceModelFactory.Create();
            var workspaceWriter = new BinaryFileWorkspaceWriter();
            workspaceWriter.Write(filePath, workspaceModel);
        }
    }
}
