using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class VariableVisualizerViewModelTests
    {
        private WorkspaceViewModel workspace;
        private Mock<IDataService> dataServiceMock;
        private Mock<IWindowManager> windowManagerMock;
        private Mock<IEventAggregator> eventAggregator;
        private VariableModel xVariable;
        private Mock<IViewModelService> viewModelCacheMock;

        [SetUp]
        public void Initialize()
        {
            this.dataServiceMock = new Mock<IDataService>();
            this.dataServiceMock.Setup(_ => _.GetWorkspace()).Returns(new WorkspaceModel());
            this.eventAggregator = new Mock<IEventAggregator>();
            this.windowManagerMock = new Mock<IWindowManager>();
            this.viewModelCacheMock = new Mock<IViewModelService>();
            this.workspace = CreateWorkspaceViewModel();
            var xVariableViewModel = this.workspace.GetModel().GetVariableByName("x");
            this.xVariable = xVariableViewModel.Model;
            this.viewModelCacheMock.Setup(_ => _.GetAllVariables())
                .Returns(new List<VariableViewModel> { xVariableViewModel });
            this.viewModelCacheMock.Setup(_ => _.GetVariableByIdentity(It.IsAny<int>()))
                .Returns(xVariableViewModel);
        }

        [Test]
        public void CreateVisualizerWithVariableBindingSetsSelectedVariable()
        {
            var sut = CreateSut();
            Assert.That(sut.SelectedVariable.Name, Is.EqualTo("x"));
        }

        private VariableVisualizerDesignViewModel CreateSut()
        {
            var visualizerModel = new VariableVisualizerModel(new Point());
            visualizerModel.BindTo(this.xVariable);
            return new VariableVisualizerDesignViewModel(visualizerModel,
                                                         this.eventAggregator.Object,
                                                         this.dataServiceMock.Object,
                                                         this.viewModelCacheMock.Object);
        }

        private WorkspaceViewModel CreateWorkspaceViewModel()
        {
            var x = new WorkspaceViewModel(this.dataServiceMock.Object,
                                           this.windowManagerMock.Object,
                                           this.eventAggregator.Object,
                                           this.viewModelCacheMock.Object);
            x.AddSingletonVariable("x", new Point());
            return x;
        }
    }
}
