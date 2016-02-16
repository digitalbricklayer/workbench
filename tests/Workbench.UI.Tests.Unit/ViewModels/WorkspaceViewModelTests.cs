using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class WorkspaceViewModelTests
    {
        private Mock<IDataService> dataServiceMock;
        private Mock<IWindowManager> windowManagerMock;
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IViewModelFactory> viewModelFactoryMock;
        private WorkspaceModel workspaceModel;

        [SetUp]
        public void Initialize()
        {
            this.workspaceModel = WorkspaceModelFactory.Create();
            this.dataServiceMock = CreateDataServiceMock();
            this.windowManagerMock = new Mock<IWindowManager>();
            this.eventAggregatorMock = new Mock<IEventAggregator>();
            this.viewModelFactoryMock = CreateViewModelMock();
        }

        [Test]
        public void SolveModelWithValidModelDisplaysSolution()
        {
            var sut = CreateSut();
            sut.SolveModel();
            Assert.That(sut.SelectedDisplayMode, Is.EqualTo("Solution"));
        }

        [Test]
        public void SolveModelWithValidModelVisualizerHasValue()
        {
            var sut = CreateSut();
            sut.SolveModel();
            var actualVisualizer = sut.Viewer.GetVisualizerFor("x");
            Assert.That(actualVisualizer.Value.Value, Is.GreaterThan(1)
                                                        .And
                                                        .LessThanOrEqualTo(10));
        }

        private WorkspaceViewModel CreateSut()
        {
            var workspaceMapper = new WorkspaceMapper(this.windowManagerMock.Object,
                                                      this.viewModelFactoryMock.Object,
                                                      this.eventAggregatorMock.Object,
                                                      this.dataServiceMock.Object);
            return workspaceMapper.MapFrom(this.workspaceModel);
        }

        private Mock<IDataService> CreateDataServiceMock()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(_ => _.GetWorkspace())
                .Returns(this.workspaceModel);
            return mock;
        }

        private Mock<IViewModelFactory> CreateViewModelMock()
        {
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateWorkspace())
                .Returns(CreateWorkspaceViewModel);
            return mock;
        }

        private WorkspaceViewModel CreateWorkspaceViewModel()
        {
            return new WorkspaceViewModel(this.dataServiceMock.Object,
                                          this.windowManagerMock.Object,
                                          this.eventAggregatorMock.Object);
        }
    }
}
