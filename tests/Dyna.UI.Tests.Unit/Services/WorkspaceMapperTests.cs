using DynaApp.Services;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.Services
{
    [TestFixture]
    public class WorkspaceMapperTests
    {
        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_View_Model()
        {
            var sut = BuildSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel, Is.Not.Null);
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
            return new WorkspaceMapper(new ModelViewModelCache());
        }
    }
}
