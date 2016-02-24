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
            Assert.That(sut.SelectedDisplayMode, Is.EqualTo("Solution"));
        }

        [Test]
        public void AddVisualizerAssignsIdentity()
        {
            var sut = CreateSut();
            var actualVisualizer = sut.Viewer.GetVisualizerFor("x");
            Assert.That(actualVisualizer.Model.HasIdentity, Is.True);
        }

        [Test]
        public void SolveModelWithVisualizerBindsValueToVisualizer()
        {
            var sut = CreateSut();
            sut.SolveModel();
            var actualVisualizer = sut.Viewer.GetVisualizerFor("x");
            Assert.That(actualVisualizer.Value.GetValueAt(1), Is.GreaterThan(1)
                                                                .And
                                                                .LessThanOrEqualTo(10));
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
            newWorkspace.AddConstraint("X", new Point());
            var theConstraint = newWorkspace.Model.GetConstraintByName("X");
            theConstraint.Expression.Text = "x > 1";
            var theVisualizer = new VariableVisualizerModel(new Point());
            var newViewer = new VariableVisualizerViewerViewModel(theVisualizer, this.eventAggregator);
            newWorkspace.AddViewer(newViewer);
            var newDesigner = new VariableVisualizerDesignViewModel(theVisualizer,
                                                                    this.eventAggregator,
                                                                    this.dataService,
                                                                    this.viewModelMock.Object);
            newWorkspace.AddDesigner(newDesigner);
            newDesigner.SelectedVariable = this.xVariable;

            return newWorkspace;
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateWorkspace())
                .Returns(null as WorkspaceViewModel);
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
