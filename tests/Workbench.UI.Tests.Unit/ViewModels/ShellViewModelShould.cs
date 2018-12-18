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

        public ShellViewModel Shell { get; private set; }

        [SetUp]
        public void Initialize()
        {
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _documentManagerMock = CreateDocumentManagerMock();
            Shell = CreateSut();
            ScreenExtensions.TryActivate(Shell);
        }

        [TearDown]
        public void Teardown()
        {
            ScreenExtensions.TryDeactivate(Shell, close: true);
        }

        [Test]
        public void CreatesCurrentDocument()
        {
            Assert.That(Shell.CurrentDocument, Is.Not.Null);
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
    }
}
