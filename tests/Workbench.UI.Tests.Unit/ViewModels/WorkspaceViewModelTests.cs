using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Core;
using Workbench.Core.Solvers;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    /// <summary>
    /// Fixture to test a workspace view model.
    /// </summary>
    [TestFixture]
    public class WorkspaceViewModelTests
    {
        private IDataService dataService;
        private Mock<IWindowManager> windowManagerMock;
        private IEventAggregator eventAggregator;

        [SetUp]
        public void Initialize()
        {
            this.dataService = new DataService(CreateWorkspaceReaderWriterMock().Object);
            this.windowManagerMock = new Mock<IWindowManager>();
            this.eventAggregator = new EventAggregator();
        }

        [Test]
        public void SolveModelWithVisualizerReturnsSuccess()
        {
            var sut = CreateSut();
            var actualResult = sut.SolveModel();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        private WorkspaceViewModel CreateSut()
        {
            var newWorkspace = new WorkspaceViewModel(this.dataService,
                                                      this.windowManagerMock.Object,
                                                      this.eventAggregator,
                                                      CreateViewModelFactoryMock().Object,
                                                      CreateModelValidator());
            ScreenExtensions.TryActivate(newWorkspace);
            newWorkspace.AddSingletonVariable(new SingletonVariableBuilder().WithName("x")
                                                                            .WithDomain("1..2")
                                                                            .Inside(newWorkspace.WorkspaceModel.Model)
                                                                            .Build());
            newWorkspace.AddExpressionConstraint(new ExpressionConstraintBuilder().WithName("X")
                                                                                  .Inside(newWorkspace.WorkspaceModel.Model)
                                                                                  .WithExpression("$x > 1")
                                                                                  .Build());

            return newWorkspace;
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            var viewModelFactoryMock = new Mock<IViewModelFactory>();
            viewModelFactoryMock.Setup(_ => _.CreateModelEditor())
                                .Returns(new ModelEditorTabViewModel(Mock.Of<IShell>(), this.dataService, this.windowManagerMock.Object, this.eventAggregator));
            return viewModelFactoryMock;
        }

        private Mock<IWorkspaceReaderWriter> CreateWorkspaceReaderWriterMock()
        {
            return new Mock<IWorkspaceReaderWriter>();
        }

        private ModelValidatorViewModel CreateModelValidator()
        {
            return new ModelValidatorViewModel(this.windowManagerMock.Object, this.dataService);
        }
    }
}
