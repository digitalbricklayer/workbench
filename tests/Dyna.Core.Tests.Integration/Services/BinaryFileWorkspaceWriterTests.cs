using System.IO;
using Dyna.Core.Models;
using DynaApp.Services;
using NUnit.Framework;

namespace Dyna.UI.Tests.Integration.Services
{
    [TestFixture]
    public class BinaryFileWorkspaceWriterTests
    {
        [Test]
        public void WriteWorkspaceToDiskThenReadBackContainsSameWorkspace()
        {
            var orginalWorkspaceModel = WorkspaceModelFactory.Create();
            var filePath = Path.GetTempFileName();
            WriteWorkspaceToDisk(filePath, orginalWorkspaceModel);
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

        private static void WriteWorkspaceToDisk(string filePath, WorkspaceModel workspaceModel)
        {
            var workspaceWriter = new BinaryFileWorkspaceWriter();
            workspaceWriter.Write(filePath, workspaceModel);
        }
    }
}
