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
        private Mock<IViewModelCache> viewModelMock;
        private VariableViewModel xVariable;

        [SetUp]
        public void Initialize()
        {
            this.dataService = new DataService(CreateWorkspaceReaderWriterMock().Object);
            this.windowManagerMock = new Mock<IWindowManager>();
            this.eventAggregator = new EventAggregator();
            this.viewModelMock = new Mock<IViewModelCache>();
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
            Assert.That(actualVisualizer.Value.Value, Is.GreaterThan(1)
                                                        .And
                                                        .LessThanOrEqualTo(10));
        }

        private WorkspaceViewModel CreateSut()
        {
            var newWorkspace = new WorkspaceViewModel(this.dataService,
                                                      this.windowManagerMock.Object,
                                                      this.eventAggregator,
                                                      this.viewModelMock.Object);
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

        private Mock<IWorkspaceReaderWriter> CreateWorkspaceReaderWriterMock()
        {
            return new Mock<IWorkspaceReaderWriter>();
        }
    }
}
