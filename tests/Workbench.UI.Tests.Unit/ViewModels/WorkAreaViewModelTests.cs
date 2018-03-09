using System.Windows;
using Caliburn.Micro;
using Moq;
using NUnit.Framework;
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
        private Mock<IViewModelService> viewModelMock;

        [SetUp]
        public void Initialize()
        {
            this.dataService = new DataService(CreateWorkspaceReaderWriterMock().Object);
            this.windowManagerMock = new Mock<IWindowManager>();
            this.eventAggregator = new EventAggregator();
            this.viewModelMock = new Mock<IViewModelService>();
        }

        [Test]
        public void SolveModelWithVisualizerDisplaysSolution()
        {
            var sut = CreateSut();
            sut.SolveModel();
            Assert.That(sut.SelectedDisplay, Is.EqualTo("Viewer"));
        }

        private WorkAreaViewModel CreateSut()
        {
            var newWorkArea = new WorkAreaViewModel(this.dataService,
                                                    this.windowManagerMock.Object,
                                                    this.eventAggregator,
                                                    this.viewModelMock.Object,
                                                    CreateViewModelFactoryMock().Object);
            newWorkArea.AddSingletonVariable(new SingletonVariableBuilder().WithName("x")
                                                                           .WithModel(newWorkArea.WorkspaceModel.Model)
                                                                           .Build(),
                                             new Point());
            var xVariable = newWorkArea.GetVariableByName("x");
            xVariable.VariableEditor.DomainExpression.Text = "1..2";
            newWorkArea.AddExpressionConstraint(new ExpressionConstraintBuilder().WithName("X")
                                                                                 .Build(),
                                                new Point());
            var theConstraint = (ExpressionConstraintEditorViewModel) newWorkArea.Editor.GetConstraintByName("X");
            theConstraint.Expression.Text = "$x > 1";

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
