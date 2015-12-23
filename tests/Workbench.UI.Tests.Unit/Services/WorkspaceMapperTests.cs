using Caliburn.Micro;
using Workbench.Services;
using Moq;
using NUnit.Framework;

namespace Workbench.UI.Tests.Unit.Services
{
    [TestFixture]
    public class WorkspaceMapperTests
    {
        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_Variables()
        {
            var sut = BuildSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.Model.Variables.Count, Is.EqualTo(2));
        }

        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_Domains()
        {
            var sut = BuildSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.Model.Domains.Count, Is.EqualTo(1));
        }

        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_Constraints()
        {
            var sut = BuildSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.Model.Constraints.Count, Is.EqualTo(1));
        }

        [Test]
        public void MapFrom_With_Valid_Model_Sets_Expected_Workspace_Model()
        {
            var sut = BuildSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.WorkspaceModel, Is.Not.Null);
        }

        private static WorkspaceMapper BuildSut()
        {
            return new WorkspaceMapper(CreateWindowManager());
        }

        private static IWindowManager CreateWindowManager()
        {
            return Mock.Of<IWindowManager>();
        }
    }
}
