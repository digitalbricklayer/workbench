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
        [Ignore("Synchronizing view model state to the model is currently broken.")]
        public void MapFrom_With_Valid_Model_Returns_Expected_Variables()
        {
            var sut = CreateSut();
            var actualWorkAreaModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkAreaModel.ModelEditor.Variables.Count, Is.EqualTo(2));
        }

        [Test]
        [Ignore("Synchronizing view model state to the model is currently broken.")]
        public void MapFrom_With_Valid_Model_Returns_Expected_Domains()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.ModelEditor.Domains.Count, Is.EqualTo(1));
        }

        [Test]
        [Ignore("Synchronizing view model state to the model is currently broken.")]
        public void MapFrom_With_Valid_Model_Returns_Expected_Constraints()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.ModelEditor.Constraints.Count, Is.EqualTo(1));
        }

        [Test]
        [Ignore("Synchronizing view model state to the model is currently broken.")]
        public void MapFrom_With_Valid_Model_Sets_Expected_Workspace_Model()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.WorkspaceModel, Is.Not.Null);
        }

        private WorkAreaMapper CreateSut()
        {
            return new WorkAreaMapper(CreateSolutionMapper(),
                                      CreateDisplayMapper(),
                                      CreateViewModelFactoryMock().Object);
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateWorkArea())
                .Returns(new WorkAreaViewModel(CreateDataService(),
                                               CreateWindowManager(),
                                               CreateEventAggregator(),
                                               mock.Object,
                                               new ModelEditorTabViewModel(CreateDataService())));

            return mock;
        }

        private DisplayMapper CreateDisplayMapper()
        {
            return new DisplayMapper(CreateVariableMapper(),
                                     CreateConstraintMapper(),
                                     CreateDomainMapper(),
                                     CreateViewModelFactoryMock().Object,
                                     this.viewModelService,
                                     CreateDataService());
        }

        private SolutionMapper CreateSolutionMapper()
        {
            return new SolutionMapper(this.viewModelService,
                                      CreateEventAggregator());
        }

        private DomainMapper CreateDomainMapper()
        {
            return new DomainMapper(this.viewModelService);
        }

        private ConstraintMapper CreateConstraintMapper()
        {
            return new ConstraintMapper(this.viewModelService);
        }

        private VariableMapper CreateVariableMapper()
        {
            return new VariableMapper(this.viewModelService,
                                      CreateEventAggregator());
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
