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
        private Mock<IEventAggregator> eventAggregatorMock;
        private VariableModel xVariable;
        private Mock<IViewModelService> viewModelCacheMock;
        private Mock<IViewModelFactory> viewModelFactoryMock;

        [SetUp]
        public void Initialize()
        {
            this.dataServiceMock = new Mock<IDataService>();
            this.dataServiceMock.Setup(_ => _.GetWorkspace())
                                .Returns(new WorkspaceModel());
            this.viewModelFactoryMock = CreateViewModelFactoryMock();

            this.eventAggregatorMock = new Mock<IEventAggregator>();
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
        [Ignore("Have broken the visualizer view models.")]
        public void CreateVisualizerWithVariableBindingSetsSelectedVariable()
        {
            var sut = CreateSut();
            Assert.That(sut.SelectedVariable.Name, Is.EqualTo(this.xVariable.Name));
        }

        private VariableVisualizerDesignViewModel CreateSut()
        {
            var visualizerModel = new VariableVisualizerModel(new Point());
            visualizerModel.BindTo(this.xVariable);
            return new VariableVisualizerDesignViewModel(visualizerModel,
                                                         this.eventAggregatorMock.Object,
                                                         this.dataServiceMock.Object,
                                                         this.viewModelCacheMock.Object);
        }

        private WorkspaceViewModel CreateWorkspaceViewModel()
        {
            var newWorkspace = new WorkspaceViewModel(this.dataServiceMock.Object,
                                                      this.windowManagerMock.Object,
                                                      this.eventAggregatorMock.Object,
                                                      this.viewModelCacheMock.Object,
                                                      this.viewModelFactoryMock.Object);
            newWorkspace.AddSingletonVariable("x", new Point());
            return newWorkspace;
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateWorkspace())
                .Returns(CreateWorkspaceViewModel);
            mock.Setup(_ => _.CreateModel(It.IsAny<ModelModel>()))
                .Returns((ModelModel model) => new ModelViewModel(model,
                                                                  this.windowManagerMock.Object,
                                                                  this.eventAggregatorMock.Object));
            return mock;
        }
    }
}
