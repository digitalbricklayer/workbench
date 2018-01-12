using System.Windows;
using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    /// <summary>
    /// Fixture to test using an empty workspace.
    /// </summary>
    [TestFixture]
    public class WorkspaceViewModelTests
    {
        private IDataService dataService;
        private Mock<IWindowManager> windowManagerMock;
        private IEventAggregator eventAggregator;
        private Mock<IViewModelService> viewModelMock;
        private VariableViewModel xVariable;

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
            Assert.That(sut.SelectedDisplay, Is.EqualTo("Solution"));
        }

        private WorkspaceViewModel CreateSut()
        {
            var newWorkspace = new WorkspaceViewModel(this.dataService,
                                                      this.windowManagerMock.Object,
                                                      this.eventAggregator,
                                                      this.viewModelMock.Object,
                                                      CreateViewModelFactoryMock().Object);
            newWorkspace.AddSingletonVariable("x", new Point());
            this.xVariable = newWorkspace.Model.GetVariableByName("x");
            this.xVariable.DomainExpression.Text = "1..2";
            newWorkspace.AddExpressionConstraint("X", new Point());
            var theConstraint = (ExpressionConstraintViewModel) newWorkspace.Model.GetConstraintByName("X");
            theConstraint.Expression.Text = "$x > 1";

            return newWorkspace;
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateModel(It.IsAny<ModelModel>()))
                .Returns((ModelModel model) => new ModelViewModel(model,
                                                                  this.windowManagerMock.Object,
                                                                  this.eventAggregator));
            return mock;
        }

        private Mock<IWorkspaceReaderWriter> CreateWorkspaceReaderWriterMock()
        {
            return new Mock<IWorkspaceReaderWriter>();
        }
    }
}
