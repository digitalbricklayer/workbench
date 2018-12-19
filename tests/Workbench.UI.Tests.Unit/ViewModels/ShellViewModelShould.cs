using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ShellViewModelShould
    {
        private Mock<IEventAggregator> _eventAggregatorMock;
        private Mock<IDocumentManager> _documentManagerMock;
        private Mock<IDataService> _dataServiceMock;
        private Mock<IWindowManager> _windowManagerMock;
        private Mock<IViewModelFactory> _viewModelFactoryMock;
        private Mock<IWorkspaceLoader> _workspaceLoader;

        public ShellViewModel Shell { get; private set; }

        [SetUp]
        public void Initialize()
        {
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _documentManagerMock = CreateDocumentManagerMock();
            _dataServiceMock = CreateDataServiceMock();
            _windowManagerMock = new Mock<IWindowManager>();
            _eventAggregatorMock = CreateEventAggregatorMock();
            _viewModelFactoryMock = CreateViewModelFactoryMock();
            _workspaceLoader = new Mock<IWorkspaceLoader>();
            Shell = CreateSut();
            ScreenExtensions.TryActivate(Shell);
        }

        [TearDown]
        public void Teardown()
        {
            ScreenExtensions.TryDeactivate(Shell, close: true);
        }

        [Test]
        public void CreateCurrentDocument()
        {
            Assert.That(Shell.CurrentDocument, Is.Not.Null);
        }

        [Test]
        public void OpenDocumentCurrentDocumentIsNewIsTrue()
        {
            var newDocument = CreateWorkspaceDocument();
            Shell.OpenDocument(newDocument);
            Assert.That(newDocument.IsNew, Is.True);
            Shell.CloseDocument();
        }

        [Test]
        public void OpenDocumentCurrentDocumentIsDirtyIsFalse()
        {
            var newDocument = CreateWorkspaceDocument();
            Shell.OpenDocument(newDocument);
            Assert.That(newDocument.IsDirty, Is.False);
            Shell.CloseDocument();
        }

        [Test]
        public void OpenDocumentCurrentDocumentWorkspaceNotNull()
        {
            var newDocument = CreateWorkspaceDocument();
            Shell.OpenDocument(newDocument);
            Assert.That(newDocument.Workspace, Is.Not.Null);
            Shell.CloseDocument();
        }

        private ShellViewModel CreateSut()
        {
            var shellViewModel = new ShellViewModel(_documentManagerMock.Object, CreateApplicationMenuMock().Object, _eventAggregatorMock.Object);

            return shellViewModel;
        }

        private Mock<IApplicationMenu> CreateApplicationMenuMock()
        {
            return new Mock<IApplicationMenu>();
        }

        private Mock<IDocumentManager> CreateDocumentManagerMock()
        {
            var mock = new Mock<IDocumentManager>();
            mock.Setup(manager => manager.CreateDocument()).Returns(CreateDocumentMock().Object);
            return mock;
        }

        private Mock<IWorkspaceDocument> CreateDocumentMock()
        {
            var mock = new Mock<IWorkspaceDocument>();
            mock.Setup(document => document.Workspace).Returns(Mock.Of<IWorkspace>());
            return mock;
        }

        private WorkspaceViewModel CreateWorkspace()
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

        private WorkspaceDocumentViewModel CreateWorkspaceDocument()
        {
            return new WorkspaceDocumentViewModel(CreateWorkspace(), _dataServiceMock.Object, _eventAggregatorMock.Object, _workspaceLoader.Object);
        }
    }
}
