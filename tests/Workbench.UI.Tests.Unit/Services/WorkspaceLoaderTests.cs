using Caliburn.Micro;
using Workbench.Services;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.Services
{
    [TestFixture]
    public class WorkspaceLoaderTests
    {
        [Test]
        public void Load_With_Valid_Model_Returns_Expected_Variables()
        {
            var sut = CreateSut();
            var actualWorkspaceViewModel = sut.Load(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceViewModel.ModelEditor.Variables.Count, Is.EqualTo(2));
        }

        [Test]
        public void Load_With_Valid_Model_Returns_Expected_Domains()
        {
            var sut = CreateSut();
            var actualWorkspaceViewModel = sut.Load(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceViewModel.ModelEditor.Domains.Count, Is.EqualTo(1));
        }

        [Test]
        public void Load_With_Valid_Model_Returns_Expected_Constraints()
        {
            var sut = CreateSut();
            var actualWorkspaceViewModel = sut.Load(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceViewModel.ModelEditor.Constraints.Count, Is.EqualTo(1));
        }

        [Test]
        public void Load_With_Valid_Model_Sets_Expected_Workspace_Model()
        {
            var sut = CreateSut();
            var actualWorkspaceViewModel = sut.Load(WorkspaceModelFactory.Create());
            Assert.That(actualWorkspaceViewModel.WorkspaceModel, Is.Not.Null);
        }

        private WorkspaceLoader CreateSut()
        {
            return new WorkspaceLoader(CreateModelEditorLoader(),
                                       CreateViewModelFactoryMock().Object,
                                       CreateWindowManager());
        }

        private ModelEditorLoader CreateModelEditorLoader()
        {
            return new ModelEditorLoader(CreateVariableMapper(), CreateConstraintMapper(), CreateDomainMapper(), CreateViewModelFactoryMock().Object);
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            var viewModelFactoryMock = new Mock<IViewModelFactory>();
            viewModelFactoryMock.Setup(factory => factory.CreateWorkspace())
                                .Returns(new WorkspaceViewModel(CreateDataService(),
                                                                CreateWindowManager(),
                                                                CreateEventAggregator(),
                                                                viewModelFactoryMock.Object,
                                                                new ModelValidatorViewModel(CreateWindowManager(),
                                                                                            CreateDataService())));
            viewModelFactoryMock.Setup(factory => factory.CreateModelEditor())
                                .Returns(new ModelEditorTabViewModel(Mock.Of<IShell>(),
                                                                     CreateDataService(),
                                                                     CreateWindowManager(),
                                                                     CreateEventAggregator()));

            return viewModelFactoryMock;
        }

        private SharedDomainLoader CreateDomainMapper()
        {
            return new SharedDomainLoader(CreateWindowManager());
        }

        private ConstraintLoader CreateConstraintMapper()
        {
            return new ConstraintLoader(CreateWindowManager());
        }

        private VariableLoader CreateVariableMapper()
        {
            return new VariableLoader(CreateWindowManager(), CreateEventAggregator());
        }

        private static IDataService CreateDataService()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(_ => _.GetWorkspace()).Returns(new WorkspaceModel());
            return mock.Object;
        }

        private static IEventAggregator CreateEventAggregator()
        {
            return new Mock<IEventAggregator>().Object;
        }

        private static IWindowManager CreateWindowManager()
        {
            return Mock.Of<IWindowManager>();
        }
    }
}
