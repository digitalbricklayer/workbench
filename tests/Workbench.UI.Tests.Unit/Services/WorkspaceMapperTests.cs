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
            var actualWorkAreaModel = sut.Load(WorkspaceModelFactory.Create());
            Assert.That(actualWorkAreaModel.ModelEditor.Variables.Count, Is.EqualTo(2));
        }

        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_Domains()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.Load(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.ModelEditor.Domains.Count, Is.EqualTo(1));
        }

        [Test]
        public void MapFrom_With_Valid_Model_Returns_Expected_Constraints()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.Load(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.ModelEditor.Constraints.Count, Is.EqualTo(1));
        }

        [Test]
        public void MapFrom_With_Valid_Model_Sets_Expected_Workspace_Model()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.Load(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.WorkspaceModel, Is.Not.Null);
        }

        private WorkspaceLoader CreateSut()
        {
            return new WorkspaceLoader(CreateModelEditorLoader(),
                                       CreateViewModelFactoryMock().Object);
        }

        private ModelEditorLoader CreateModelEditorLoader()
        {
            return new ModelEditorLoader(CreateVariableMapper(), CreateConstraintMapper(), CreateDomainMapper(), CreateViewModelFactoryMock().Object);
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateWorkspace())
                .Returns(new WorkspaceViewModel(CreateDataService(),
                                                CreateWindowManager(),
                                                CreateEventAggregator(),
                                                mock.Object));

            return mock;
        }

        private SharedDomainLoader CreateDomainMapper()
        {
            return new SharedDomainLoader(CreateWindowManager());
        }

        private ConstraintLoader CreateConstraintMapper()
        {
            return new ConstraintLoader(CreateWindowManager());
        }

        private VariableLoader CreateVariableMapper()
        {
            return new VariableLoader(CreateWindowManager());
        }

        private static IDataService CreateDataService()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(_ => _.GetWorkspace())
                .Returns(new WorkspaceModel());
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
