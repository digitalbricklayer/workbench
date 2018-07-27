using System;
using Caliburn.Micro;
using Workbench.ViewModels;
using Moq;
using NUnit.Framework;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class WorkspaceViewModelSolveTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private WorkspaceViewModel sut;

        [SetUp]
        public void Initialize()
        {
            this.eventAggregatorMock = new Mock<IEventAggregator>();
            this.sut = CreateSut();
            ScreenExtensions.TryActivate(this.sut);
            this.sut.ModelEditor.AddSingletonVariableCommand.Execute(new SingletonVariableBuilder().WithName("x")
                .WithDomain("1..10")
                .WithModel(this.sut.WorkspaceModel.Model)
                .Build());
            this.sut.ModelEditor.AddAggregateVariableCommand.Execute(new AggregateVariableBuilder().WithName("y")
                .WithModel(this.sut.WorkspaceModel.Model)
                .WithSize(2)
                .WithDomain("1..10")
                .Build());
            this.sut.ModelEditor.AddExpressionConstraintCommand.Execute(new ExpressionConstraintBuilder().WithExpression("$x > 1").Build());
            this.sut.ModelEditor.AddExpressionConstraintCommand.Execute(new ExpressionConstraintBuilder().WithExpression("$y[0] <> $y[1]").Build());
        }

        [TearDown]
        public void Teardown()
        {
            ScreenExtensions.TryDeactivate(this.sut, close:true);
        }

        [Test]
        public void SolveWithValidModelReturnsSuccessStatus()
        {
            var actualStatus = sut.SolveModel();
            Assert.That(actualStatus.IsSuccess, Is.True);
        }

        [Test]
        public void AddWithValidSingletonVariablePublishesVariableAddedMessage()
        {
            sut.AddSingletonVariable(new SingletonVariableBuilder().WithName("z")
                                                                   .WithModel(sut.WorkspaceModel.Model)
                                                                   .Build());
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<SingletonVariableAddedMessage>(msg => msg.NewVariableName == "z"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void AddWithValidAggregatorVariablePublishesVariableAddedMessage()
        {
            sut.AddAggregateVariable(new AggregateVariableBuilder().WithName("z")
                                                                   .WithModel(sut.WorkspaceModel.Model)
                                                                   .Build());
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<AggregateVariableAddedMessage>(msg => msg.NewVariableName == "z"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void DeleteWithValidVariablePublishesVariableDeletedMessage()
        {
            var variableToDelete = sut.WorkspaceModel.Model.GetVariableByName("x");
            sut.DeleteVariable(variableToDelete);
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<VariableDeletedMessage>(msg => msg.VariableName == "x"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        private WorkspaceViewModel CreateSut()
        {
            var workspaceViewModel = new WorkspaceViewModel(CreateDataService(), CreateWindowManagerMock().Object, this.eventAggregatorMock.Object, CreateViewModelFactoryMock().Object);

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
            var x = new Mock<IViewModelFactory>();
            x.Setup(_ => _.CreateModelEditor())
             .Returns(new ModelEditorTabViewModel(new AppRuntime{Workspace = this.sut}, CreateDataService(), CreateWindowManagerMock().Object));
            return x;
        }
    }
}
