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
            IoC.GetInstance = (type, s) => CreateWorkAreaViewModel();
            IoC.GetAllInstances = type => null;
            IoC.BuildUp = o => {};
        }

        [Test]
        public void CreateWorkspaceFiresWorkspaceCreatedEvent()
        {
            var sut = CreateSut();
            var wasWorkAreaCreatedCalled = false;
            sut.WorkAreaCreated += (o, e) => wasWorkAreaCreatedCalled = true;
            sut.CreateWorkArea();
            Assert.That(wasWorkAreaCreatedCalled, Is.True);
        }

        private static ViewModelFactory CreateSut()
        {
            return new ViewModelFactory(CreateEventAggregatorMock().Object,
                                        CreateWindowManagerMock().Object);
        }

        private object CreateWorkAreaViewModel()
        {
            return new WorkAreaViewModel(CreateDataServiceMock().Object,
                                         CreateWindowManagerMock().Object,
                                         CreateEventAggregatorMock().Object,
                                         CreateViewModelServiceMock().Object,
                                         CreateViewModelFactoryMock().Object);
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            return new Mock<IViewModelFactory>();
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
