using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    /// <summary>
    /// Fixture to test using a workspace view model created from 
    /// an existing workspace model.
    /// </summary>
    [TestFixture]
    public class WorkspaceViewModelCreatedFromModelTests
    {
        private Mock<IDataService> dataServiceMock;
        private Mock<IWindowManager> windowManagerMock;
        private Mock<IEventAggregator> eventAggregatorMock;
        private WorkspaceModel workspaceModel;
        private IViewModelService viewModelService;
        private Mock<IViewModelFactory> viewModelFactoryMock;

        [SetUp]
        public void Initialize()
        {
            this.workspaceModel = WorkspaceModelFactory.Create();
            this.dataServiceMock = CreateDataServiceMock();
            this.windowManagerMock = new Mock<IWindowManager>();
            this.eventAggregatorMock = new Mock<IEventAggregator>();
            this.viewModelFactoryMock = CreateViewModelFactoryMock();
            this.viewModelService = new ViewModelService(this.viewModelFactoryMock.Object);
        }

        [Test]
        public void SolveModelWithValidModelDisplaysSolution()
        {
            var sut = CreateSut();
            sut.SolveModel();
            Assert.That(sut.SelectedDisplayMode, Is.EqualTo("Solution"));
        }

        [Test]
        [Ignore("Have broken the visualizer view models.")]
        public void SolveModelWithValidModelVisualizerHasValue()
        {
            var sut = CreateSut();
            sut.SolveModel();
            var actualVisualizer = sut.Viewer.GetVisualizerFor("x");
            Assert.That(actualVisualizer.Value.GetValueAt(0), Is.GreaterThan(1)
                                                                .And
                                                                .LessThanOrEqualTo(10));
        }

        private WorkspaceViewModel CreateSut()
        {
            var workspaceMapper = new WorkspaceMapper(CreateModelMapper(),
                                                      CreateSolutionMapper(),
                                                      CreateDisplayMapper(),
                                                      CreateViewModelFactoryMock().Object);
            return workspaceMapper.MapFrom(this.workspaceModel);
        }

        private DisplayMapper CreateDisplayMapper()
        {
            return new DisplayMapper(CreateEventAggregatorMock().Object,
                                     CreateDataServiceMock().Object,
                                     this.viewModelService);
        }

        private SolutionMapper CreateSolutionMapper()
        {
            return new SolutionMapper(this.viewModelService,
                                      this.eventAggregatorMock.Object);
        }

        private ModelMapper CreateModelMapper()
        {
            return new ModelMapper(CreateVariableMapper(),
                                   CreateConstraintMapper(),
                                   CreateDomainMapper(),
                                   this.viewModelFactoryMock.Object);
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
            return new VariableMapper(this.viewModelService, this.eventAggregatorMock.Object);
        }

        private Mock<IEventAggregator> CreateEventAggregatorMock()
        {
            return new Mock<IEventAggregator>();
        }

        private Mock<IDataService> CreateDataServiceMock()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(_ => _.GetWorkspace())
                .Returns(this.workspaceModel);
            return mock;
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateWorkspace())
                .Returns(CreateWorkspaceViewModel);
            mock.Setup(_ => _.CreateModel(It.IsAny<ModelModel>()))
                .Returns((ModelModel model) => new ModelViewModel(model,
                                                                  this.windowManagerMock.Object,
                                                                  this.eventAggregatorMock.Object));
            return mock;
        }

        private WorkspaceViewModel CreateWorkspaceViewModel()
        {
            return new WorkspaceViewModel(this.dataServiceMock.Object,
                                          this.windowManagerMock.Object,
                                          this.eventAggregatorMock.Object,
                                          this.viewModelService,
                                          this.viewModelFactoryMock.Object);
        }
    }
}
