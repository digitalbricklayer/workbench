using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.Services
{
    [TestFixture]
    public class ViewModelFactoryTests
    {
        [SetUp]
        public void Initialize()
        {
            IoC.GetInstance = (type, s) => CreateWorkspaceViewModel();
            IoC.GetAllInstances = type => null;
            IoC.BuildUp = o => {};
        }

        [Test]
        public void CreateWorkspaceFiresWorkspaceCreatedEvent()
        {
            var sut = new ViewModelFactory();
            var wasWorkspaceCreatedCalled = false;
            sut.WorkspaceCreated += (o, e) => wasWorkspaceCreatedCalled = true;
            sut.CreateWorkspace();
            Assert.That(wasWorkspaceCreatedCalled, Is.True);
        }

        private static object CreateWorkspaceViewModel()
        {
            return new WorkspaceViewModel(CreateDataServiceMock().Object,
                                          CreateWindowManagerMock().Object,
                                          CreateEventAggregatorMock().Object,
                                          CreateViewModelServiceMock().Object);
        }

        private static Mock<IViewModelService> CreateViewModelServiceMock()
        {
            return new Mock<IViewModelService>();
        }

        private static Mock<IEventAggregator> CreateEventAggregatorMock()
        {
            return new Mock<IEventAggregator>();
        }

        private static Mock<IWindowManager> CreateWindowManagerMock()
        {
            return new Mock<IWindowManager>();
        }

        private static Mock<IDataService> CreateDataServiceMock()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(_ => _.GetWorkspace()).Returns(new WorkspaceModel());
            return mock;
        }
    }
}
