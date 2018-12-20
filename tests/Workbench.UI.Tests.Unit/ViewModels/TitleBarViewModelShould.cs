using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Messages;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class TitleBarViewModelShould
    {
        private IShell _shell;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private DocumentManager _documentManager;
        private Mock<IDataService> _dataServiceMock;
        private Mock<IWindowManager> _windowManagerMock;
        private Mock<IViewModelFactory> _viewModelFactoryMock;
        private Mock<IWorkspaceLoader> _workspaceLoader;

        [SetUp]
        public void Initialize()
        {
            _windowManagerMock = new Mock<IWindowManager>();
            _dataServiceMock = CreateDataServiceMock();
            _eventAggregatorMock = CreateEventAggregatorMock();
            _viewModelFactoryMock = CreateViewModelFactoryMock();
            _workspaceLoader = new Mock<IWorkspaceLoader>();
            _documentManager = CreateDocumentManager();
            _shell = CreateShell();
            ScreenExtensions.TryActivate(_shell);
        }

        [TearDown]
        public void TearDown()
        {
            ScreenExtensions.TryDeactivate(_shell, close: true);
        }

        [Test]
        public void TitleWithoutFileEndsWithUntitled()
        {
            var sut = new TitleBarViewModel(_shell, _eventAggregatorMock.Object);
            ScreenExtensions.TryActivate(sut);
            Assert.That(sut.Title, Does.EndWith("untitled").IgnoreCase);
            ScreenExtensions.TryDeactivate(sut, close: true);
        }

        [Test]
        public void HandleWithDocumentOpenedMessageChangesTitle()
        {
            var sut = new TitleBarViewModel(_shell, _eventAggregatorMock.Object);
            ScreenExtensions.TryActivate(sut);
            var theOpenedDocument = CreateOpenWorkspaceDocument();
            _shell.OpenDocument(theOpenedDocument);
            sut.Handle(new DocumentOpenedMessage(theOpenedDocument));
            Assert.That(sut.Title, Does.EndWith(@"nqueens.dps").IgnoreCase);
            ScreenExtensions.TryDeactivate(sut, close: true);
        }

        private ShellViewModel CreateShell()
        {
            var shellViewModel = new ShellViewModel(_documentManager, CreateApplicationMenuMock().Object, _eventAggregatorMock.Object);

            return shellViewModel;
        }

        private Mock<IApplicationMenu> CreateApplicationMenuMock()
        {
            return new Mock<IApplicationMenu>();
        }

        private DocumentManager CreateDocumentManager()
        {
            return new DocumentManager(_viewModelFactoryMock.Object);
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
            var mock = new Mock<IViewModelFactory>();
            mock.SetupSequence(_ => _.CreateDocument())
                .Returns(CreateNewWorkspaceDocument)
                .Returns(CreateOpenWorkspaceDocument);

            return mock;
        }

        private WorkspaceDocumentViewModel CreateNewWorkspaceDocument()
        {
            return new WorkspaceDocumentViewModel(CreateWorkspace(), _dataServiceMock.Object, _eventAggregatorMock.Object, _workspaceLoader.Object);
        }

        private WorkspaceDocumentViewModel CreateOpenWorkspaceDocument()
        {
            var theOpenDocument = new WorkspaceDocumentViewModel(CreateWorkspace(), _dataServiceMock.Object, _eventAggregatorMock.Object, _workspaceLoader.Object);
            theOpenDocument.Path = new DocumentPathViewModel(@"c:\some\folder\or\other\nqueens.dps");
            theOpenDocument.IsNew = false;

            return theOpenDocument;
        }
    }
}
