using System;
using Caliburn.Micro;
using Workbench.ViewModels;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class WorkAreaViewModelXTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private IViewModelService viewModelService;
        private Mock<IViewModelFactory> viewModelFactoryMock;
        private Mock<IWindowManager> windowManagerMock;

        [SetUp]
        public void Initialize()
        {
            this.eventAggregatorMock = new Mock<IEventAggregator>();
            this.viewModelFactoryMock = CreateViewModelFactoryMock();
            this.viewModelService = Mock.Of<IViewModelService>();
            this.windowManagerMock = CreateWindowManagerMock();
        }

        [Test]
        public void SolveWithValidModelReturnsSuccessStatus()
        {
            var sut = CreateValidWorkArea();
            var actualStatus = sut.SolveModel();
            Assert.That(actualStatus.IsSuccess, Is.True);
        }

        [Test]
        public void AddWithValidSingletonVariablePublishesVariableAddedMessage()
        {
            var sut = CreateValidWorkArea();
            var theSingletonVariable = new SingletonVariableModel(sut.WorkspaceModel.Model, new ModelName("z"));
            sut.AddSingletonVariable(new SingletonVariableVisualizerViewModel(theSingletonVariable,
                                                                              new SingletonVariableEditorViewModel(new SingletonVariableGraphicModel(theSingletonVariable), this.eventAggregatorMock.Object, CreateDataService(), this.viewModelService),
                                                                              new SingletonVariableViewerViewModel(new SingletonVariableGraphicModel(theSingletonVariable))));
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<SingletonVariableAddedMessage>(msg => msg.NewVariableName == "z"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void AddWithValidAggregatorVariablePublishesVariableAddedMessage()
        {
            var sut = CreateValidWorkArea();
            sut.AddAggregateVariable(new AggregateVariableBuilder().WithName("z")
                                                                   .WithModel(sut.WorkspaceModel.Model)
                                                                   .WithEventAggregator(this.eventAggregatorMock.Object)
                                                                   .Build());
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<AggregateVariableAddedMessage>(msg => msg.NewVariableName == "z"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void DeleteWithValidVariablePublishesVariableDeletedMessage()
        {
            var sut = CreateValidWorkArea();
            var variableToDelete = sut.GetVariableByName("x");
            sut.DeleteVariable(variableToDelete);
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<VariableDeletedMessage>(msg => msg.VariableName == "x"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        private WorkAreaViewModel CreateValidWorkArea()
        {
            var workspaceViewModel = new WorkAreaViewModel(CreateDataService(),
                                                           CreateWindowManagerMock().Object,
                                                           this.eventAggregatorMock.Object,
                                                           this.viewModelService,
                                                           this.viewModelFactoryMock.Object);
            var x = new SingletonVariableModel(workspaceViewModel.WorkspaceModel.Model, new ModelName("x"), new VariableDomainExpressionModel("1..10"));
            workspaceViewModel.AddSingletonVariable(new SingletonVariableVisualizerViewModel(x,
                                                                                             new SingletonVariableEditorViewModel(new SingletonVariableGraphicModel(x), this.eventAggregatorMock.Object, CreateDataService(), this.viewModelService),
                                                                                             new SingletonVariableViewerViewModel(new SingletonVariableGraphicModel(x))));
            workspaceViewModel.AddAggregateVariable(new AggregateVariableBuilder().WithName("y")
                                                                                  .WithModel(workspaceViewModel.WorkspaceModel.Model)
                                                                                  .WithSize(2)
                                                                                  .WithDomain("1..10")
                                                                                  .Build());
            workspaceViewModel.AddExpressionConstraint(new ExpressionConstraintViewModel(new ExpressionConstraintGraphicModel(new ExpressionConstraintModel(new ConstraintExpressionModel("$x > 1")))));
            workspaceViewModel.AddExpressionConstraint(new ExpressionConstraintViewModel(new ExpressionConstraintGraphicModel(new ExpressionConstraintModel(new ConstraintExpressionModel("$y[0] <> $y[1]")))));

            return workspaceViewModel;
        }

        private IDataService CreateDataService()
        {
            return new DataService(CreateWorkspaceReaderWriterMock().Object);
        }

        private Mock<IWorkspaceReaderWriter> CreateWorkspaceReaderWriterMock()
        {
            return new Mock<IWorkspaceReaderWriter>();
        }

        private static Mock<IWindowManager> CreateWindowManagerMock()
        {
            return new Mock<IWindowManager>();
        }

        private Mock<IViewModelFactory> CreateViewModelFactoryMock()
        {
            return new Mock<IViewModelFactory>();
        }
    }
}
