using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class MainWindowViewModelShould
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
        }

        [Test]
        public void ApplicationIconIsNotNull()
        {
            var titleBar = new TitleBarViewModel(_shell, _eventAggregatorMock.Object);
            var sut = new MainWindowViewModel(_shell, titleBar, CreateResourceManager().Object);
            ScreenExtensions.TryActivate(sut);
            Assert.That(sut.Icon, Is.Not.Null);
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

        private Mock<IResourceManager> CreateResourceManager()
        {
            var x = new Mock<IResourceManager>();
            x.Setup(_ => _.GetBitmap(It.IsAny<string>())).Returns(new BitmapImage());
            x.Setup(_ => _.GetBitmap(It.IsAny<string>(), It.IsAny<string>())).Returns(new BitmapImage());
            return x;
        }
    }
}
