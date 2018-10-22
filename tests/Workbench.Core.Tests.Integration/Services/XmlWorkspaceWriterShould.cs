using System.IO;
using Workbench.Services;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.UI.Tests.Integration.Services
{
    [TestFixture]
    public class XmlWorkspaceWriterShould
    {
        [Test]
        public void WriteWorkspaceToDiskThenReadBackIn()
        {
            var filePath = Path.GetTempFileName();
            WriteWorkspaceToDisk(filePath);
            var readWorkspaceModel = ReadWorkspaceModel(filePath);
            Assert.That(readWorkspaceModel, Is.Not.Null);
            Assert.That(readWorkspaceModel.Model.Name.Text, Is.Not.Empty);
            var variableX = readWorkspaceModel.Model.GetVariableByName("x");
            Assert.That(variableX.Name.Text, Is.EqualTo("x"));
            File.Delete(filePath);
        }

        private static WorkspaceModel ReadWorkspaceModel(string filePath)
        {
            var workspaceReader = new XmlWorkspaceReader();
            return workspaceReader.Read(filePath);
        }

        private static void WriteWorkspaceToDisk(string filePath)
        {
            var workspaceModel = new WorkspaceModelFactory().Create();
            var workspaceWriter = new XmlWorkspaceWriter();
            workspaceWriter.Write(filePath, workspaceModel);
        }
    }
}
