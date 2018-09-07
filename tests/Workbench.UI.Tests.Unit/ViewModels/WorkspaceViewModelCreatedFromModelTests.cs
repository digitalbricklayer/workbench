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
        private Mock<IDataService> _dataServiceMock;
        private Mock<IWindowManager> _windowManagerMock;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private WorkspaceModel _workspaceModel;
        private Mock<IViewModelFactory> _viewModelFactoryMock;

        [SetUp]
        public void Initialize()
        {
            _workspaceModel = WorkspaceModelFactory.Create();
            _dataServiceMock = CreateDataServiceMock();
            _windowManagerMock = new Mock<IWindowManager>();
            _eventAggregatorMock = CreateEventAggregatorMock();
            _viewModelFactoryMock = CreateViewModelFactoryMock();
        }
    
        [Test]
        public void SolveModelWithValidModelDisplaysSolution()
        {
            var sut = CreateSut();
            sut.SolveModel();
            Assert.That(sut.ActiveItem.DisplayName, Is.EqualTo("Viewer"));
        }

        private WorkspaceViewModel CreateSut()
        {
            return new WorkspaceViewModel(_dataServiceMock.Object, _windowManagerMock.Object, _eventAggregatorMock.Object, _viewModelFactoryMock.Object);

        }

        private Mock<IEventAggregator> CreateEventAggregatorMock()
        {
            return new Mock<IEventAggregator>();
        }

        private Mock<IDataService> CreateDataServiceMock()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(_ => _.GetWorkspace())
                .Returns(_workspaceModel);
            return mock;
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateWorkspace()).Returns(CreateWorkspaceViewModel);
            return mock;
        }

        private WorkspaceViewModel CreateWorkspaceViewModel()
        {
            return new WorkspaceViewModel(_dataServiceMock.Object,
                                          _windowManagerMock.Object,
                                          _eventAggregatorMock.Object,
                                          _viewModelFactoryMock.Object);
        }
    }
}
