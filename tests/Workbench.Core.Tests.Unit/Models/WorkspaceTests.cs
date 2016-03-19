using FluentAssertions;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class WorkspaceTests
    {
        [Test]
        public void Create_Workspace_With_Blank_Name_Returns_Model_With_Blank_Name()
        {
            var sut = WorkspaceModelFactory.Create();
            Assert.That(sut.Model.Name, Is.Empty);
        }

        [Test]
        public void WorkspaceModelIsSerializable()
        {
            var sut = WorkspaceModelFactory.Create();
            sut.Should().BeBinarySerializable();
        }

        [Test]
        public void Create_Workspace_With_Name_Returns_Model_With_Expected_Name()
        {
            const string ExpectedModelName = "The expected model name";
            var sut = WorkspaceModel.Create(ExpectedModelName)
                                    .AddSingleton("x", "1..10")
                                    .WithConstraint("x > 1")
                                    .WithVariableVisualizerBindingTo("x")
                                    .Build();
            Assert.That(sut.Model.Name, Is.EqualTo(ExpectedModelName));
        }
    }
}
