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
        private IViewModelService viewModelService;

        [SetUp]
        public void Initialize()
        {
            var viewModelFactory = new ViewModelFactory(CreateEventAggregator(),
                                                        CreateWindowManager());
            this.viewModelService = new ViewModelService(viewModelFactory);
        }

        [Test]
        [Ignore("Synchronizing view model state to the model is currently broken.")]
        public void MapFrom_With_Valid_Model_Returns_Expected_Variables()
        {
            var sut = CreateSut();
            var actualWorkAreaModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkAreaModel.Editor.Variables.Count, Is.EqualTo(2));
        }

        [Test]
        [Ignore("Synchronizing view model state to the model is currently broken.")]
        public void MapFrom_With_Valid_Model_Returns_Expected_Domains()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.Editor.Domains.Count, Is.EqualTo(1));
        }

        [Test]
        [Ignore("Synchronizing view model state to the model is currently broken.")]
        public void MapFrom_With_Valid_Model_Returns_Expected_Constraints()
        {
            var sut = CreateSut();
            var actualWorkspaceModel = sut.MapFrom(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceModel.Editor.Constraints.Count, Is.EqualTo(1));
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
                                                this.viewModelService,
                                                mock.Object));

            return mock;
        }

        private DisplayMapper CreateDisplayMapper()
        {
            return new DisplayMapper(CreateVariableMapper(),
                                     CreateConstraintMapper(),
                                     CreateDomainMapper(),
                                     CreateViewModelFactoryMock().Object,
                                     this.viewModelService);
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
