using System;
using Caliburn.Micro;
using Workbench.ViewModels;
using Moq;
using NUnit.Framework;
using Workbench.Core;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class WorkspaceViewModelSolveTests
    {
        private Mock<IEventAggregator> _eventAggregatorMock;
        private readonly IAppRuntime _appRuntime = new AppRuntime();

        public WorkspaceViewModel Workspace { get; private set; }

        [SetUp]
        public void Initialize()
        {
            _eventAggregatorMock = new Mock<IEventAggregator>();
            Workspace = CreateSut();
            _appRuntime.Workspace = Workspace;
            ScreenExtensions.TryActivate(Workspace);
            Workspace.AddSingletonVariable(new SingletonVariableBuilder().WithName("x")
                                                                         .WithDomain("1..10")
                                                                         .WithModel(Workspace.WorkspaceModel.Model)
                                                                         .Build());
            Workspace.AddAggregateVariable(new AggregateVariableBuilder().WithName("y")
                                                                         .WithModel(Workspace.WorkspaceModel.Model)
                                                                         .WithSize(2)
                                                                         .WithDomain("1..10")
                                                                         .Build());
            Workspace.AddExpressionConstraint(new ExpressionConstraintBuilder().WithExpression("$x > 1")
                                                                               .Build());
            Workspace.AddExpressionConstraint(new ExpressionConstraintBuilder().WithExpression("$y[0] <> $y[1]")
                                                                               .Build());
        }

        [TearDown]
        public void Teardown()
        {
            ScreenExtensions.TryDeactivate(Workspace, close:true);
        }

        [Test]
        public void SolveWithValidModelReturnsSuccessStatus()
        {
            var actualStatus = Workspace.SolveModel();
            Assert.That(actualStatus.IsSuccess, Is.True);
        }

        [Test]
        public void AddWithValidSingletonVariablePublishesVariableAddedMessage()
        {
            Workspace.AddSingletonVariable(new SingletonVariableBuilder().WithName("z")
                                                                         .WithModel(Workspace.WorkspaceModel.Model)
                                                                         .Build());
            _eventAggregatorMock.Verify(_ => _.Publish(It.Is<SingletonVariableAddedMessage>(msg => msg.NewVariableName == "z"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void AddWithValidAggregatorVariablePublishesVariableAddedMessage()
        {
            Workspace.AddAggregateVariable(new AggregateVariableBuilder().WithName("z")
                                                                         .WithModel(Workspace.WorkspaceModel.Model)
                                                                         .Build());
            _eventAggregatorMock.Verify(_ => _.Publish(It.Is<AggregateVariableAddedMessage>(msg => msg.NewVariableName == "z"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void DeleteWithValidVariablePublishesVariableDeletedMessage()
        {
            var variableToDelete = Workspace.ModelEditor.GetVariableByName("x");
            Workspace.ModelEditor.DeleteVariable(variableToDelete);
            _eventAggregatorMock.Verify(_ => _.Publish(It.Is<VariableDeletedMessage>(msg => msg.VariableName == "x"), It.IsAny<Action<System.Action>>()),
                                        Times.Once);
        }

        private WorkspaceViewModel CreateSut()
        {
            var workspaceViewModel = new WorkspaceViewModel(CreateDataService(), CreateWindowManagerMock().Object, this._eventAggregatorMock.Object, CreateViewModelFactoryMock().Object);

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
            var viewModelFactoryMock = new Mock<IViewModelFactory>();
            viewModelFactoryMock.Setup(_ => _.CreateModelEditor())
                                .Returns(new ModelEditorTabViewModel(_appRuntime, CreateDataService(), CreateWindowManagerMock().Object, _eventAggregatorMock.Object));
            return viewModelFactoryMock;
        }
    }
}
