using Caliburn.Micro;
using Workbench.Services;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.Services
{
    [TestFixture]
    public class WorkspaceMapperTests
    {
        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_Variables()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.Model.Variables.Count, Is.EqualTo(2));
        }

        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_Domains()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.Model.Domains.Count, Is.EqualTo(1));
        }

        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_Constraints()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.Model.Constraints.Count, Is.EqualTo(1));
        }

        [Test]
        public void MapFrom_With_Valid_Model_Sets_Expected_Workspace_Model()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.WorkspaceModel, Is.Not.Null);
        }

        private static WorkspaceMapper CreateSut()
        {
            return new WorkspaceMapper(CreateWindowManager(),
                                       CreateViewModelFactory(),
                                       CreateEventAggregator());
        }

        private static IViewModelFactory CreateViewModelFactory()
        {
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateWorkspace())
                .Returns(CreateNewWorkspace());
            return mock.Object;
        }

        private static WorkspaceViewModel CreateNewWorkspace()
        {
            return new WorkspaceViewModel(CreateDataService(),
                                          CreateWindowManager(),
                                          CreateEventAggregator());
        }

        private static IDataService CreateDataService()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(_ => _.GetWorkspace()).Returns(new WorkspaceModel());
            return mock.Object;
        }

        private static IEventAggregator CreateEventAggregator()
        {
            return new Mock<IEventAggregator>().Object;
        }

        private static IWindowManager CreateWindowManager()
        {
            return Mock.Of<IWindowManager>();
        }
    }
}
