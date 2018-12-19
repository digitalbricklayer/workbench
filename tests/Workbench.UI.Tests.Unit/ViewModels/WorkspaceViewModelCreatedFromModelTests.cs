using Caliburn.Micro;
using Moq;
using NUnit.Framework;
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
        private Mock<IDataService> _dataServiceMock;
        private Mock<IWindowManager> _windowManagerMock;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private Mock<IViewModelFactory> _viewModelFactoryMock;

        /// <summary>
        /// Gets the subject of the test.
        /// </summary>
        public WorkspaceViewModel Subject { get; private set; }

        [SetUp]
        public void Initialize()
        {
            _dataServiceMock = CreateDataServiceMock();
            _windowManagerMock = new Mock<IWindowManager>();
            _eventAggregatorMock = CreateEventAggregatorMock();
            _viewModelFactoryMock = CreateViewModelFactoryMock();
            Subject = CreateSut();
            ScreenExtensions.TryActivate(Subject);
        }

        [Test]
        public void SolveModelWithValidModelDisplaysSolutionTab()
        {
            Subject.SolveModel();
            Assert.That(Subject.ActiveItem, Is.InstanceOf<SolutionViewerTabViewModel>());
        }

        [Test]
        public void SolveModelWithValidModelBindsLabels()
        {
            Subject.SolveModel();
            var solutionViewerTab = Subject.ActiveItem as SolutionViewerTabViewModel;
            Assert.That(solutionViewerTab.Viewer.Labels, Is.Not.Empty);
        }

        private WorkspaceViewModel CreateSut()
        {
            return new WorkspaceViewModel(_dataServiceMock.Object,
                                          _windowManagerMock.Object,
                                          _eventAggregatorMock.Object,
                                          _viewModelFactoryMock.Object,
                                          new ModelValidatorViewModel(_windowManagerMock.Object, _dataServiceMock.Object));
        }

        private Mock<IEventAggregator> CreateEventAggregatorMock()
        {
            return new Mock<IEventAggregator>();
        }

        private Mock<IDataService> CreateDataServiceMock()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(_ => _.GetWorkspace()).Returns(WorkspaceModelFactory.Create);
            return mock;
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            return new Mock<IViewModelFactory>();
        }
    }
}
