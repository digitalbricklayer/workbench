using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Core.Solver;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    /// <summary>
    /// Fixture to test using an empty workspace.
    /// </summary>
    [TestFixture]
    public class WorkAreaViewModelTests
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

        private WorkAreaViewModel CreateSut()
        {
            var newWorkArea = new WorkAreaViewModel(this.dataService,
                                                    this.windowManagerMock.Object,
                                                    this.eventAggregator,
                                                    CreateViewModelFactoryMock().Object,
                                                    new ModelEditorTabViewModel(this.dataService));
            newWorkArea.AddSingletonVariable(new SingletonVariableBuilder().WithName("x")
                .WithDomain("1..2")
                    .WithModel(newWorkArea.WorkspaceModel.Model)
                    .Build());
            newWorkArea.AddExpressionConstraint(new ExpressionConstraintBuilder().WithName("X").WithExpression("$x > 1")
                    .Build());

            return newWorkArea;
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            return new Mock<IViewModelFactory>();
        }

        private Mock<IWorkspaceReaderWriter> CreateWorkspaceReaderWriterMock()
        {
            return new Mock<IWorkspaceReaderWriter>();
        }
    }
}
