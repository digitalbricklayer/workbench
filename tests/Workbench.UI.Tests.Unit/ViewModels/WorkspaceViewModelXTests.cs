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
    public class WorkspaceViewModelXTests
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
            var sut = CreateValidWorkspace();
            var actualStatus = sut.SolveModel();
            Assert.That(actualStatus.IsSuccess, Is.True);
        }

        [Test]
        public void AddWithValidSingletonVariablePublishesVariableAddedMessage()
        {
            var sut = CreateValidWorkspace();
            sut.Model.AddSingletonVariable(new VariableViewModel(new VariableModel("z"), this.eventAggregatorMock.Object));
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<SingletonVariableAddedMessage>(msg => msg.NewVariableName == "z"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void AddWithValidAggregatorVariablePublishesVariableAddedMessage()
        {
            var sut = CreateValidWorkspace();
            sut.Model.AddAggregateVariable(new AggregateVariableViewModel(new AggregateVariableModel("z"), this.eventAggregatorMock.Object));
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<AggregateVariableAddedMessage>(msg => msg.NewVariableName == "z"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void DeleteWithValidVariablePublishesVariableDeletedMessage()
        {
            var sut = CreateValidWorkspace();
            var variableToDelete = sut.Model.GetVariableByName("x");
            sut.DeleteVariable(variableToDelete);
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<VariableDeletedMessage>(msg => msg.VariableName == "x"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        private WorkspaceViewModel CreateValidWorkspace()
        {
            var workspaceViewModel = new WorkspaceViewModel(CreateDataService(),
                                                            CreateWindowManagerMock().Object,
                                                            this.eventAggregatorMock.Object,
                                                            this.viewModelService,
                                                            this.viewModelFactoryMock.Object);
            workspaceViewModel.Model.AddSingletonVariable(new VariableViewModel(new VariableModel("x", new VariableDomainExpressionModel("1..10")), Mock.Of<IEventAggregator>()));
            workspaceViewModel.Model.AddAggregateVariable(new AggregateVariableViewModel(new AggregateVariableModel("y", 2, new VariableDomainExpressionModel("1..10")), Mock.Of<IEventAggregator>()));
            workspaceViewModel.Model.AddConstraint(new ConstraintViewModel(new ExpressionConstraintModel("x", "x > 1")));
            workspaceViewModel.Model.AddConstraint(new ConstraintViewModel(new ExpressionConstraintModel("aggregates must be different",
                                                                                               "y[1] <> y[2]")));

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
            var mock = new Mock<IViewModelFactory>();
            mock.Setup(_ => _.CreateModel(It.IsAny<ModelModel>()))
                .Returns((ModelModel model) => new ModelViewModel(model,
                                                                  this.windowManagerMock.Object,
                                                                  this.eventAggregatorMock.Object));
            return mock;
        }
    }
}
