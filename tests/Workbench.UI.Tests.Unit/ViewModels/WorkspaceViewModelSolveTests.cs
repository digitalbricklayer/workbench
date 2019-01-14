using System;
using Caliburn.Micro;
using Workbench.ViewModels;
using Moq;
using NUnit.Framework;
using Workbench.Core;
using Workbench.Core.Models;
using Workbench.Core.Solver;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class WorkspaceViewModelSolveTests
    {
        private Mock<IEventAggregator> _eventAggregatorMock;

        public WorkspaceViewModel Workspace { get; private set; }

        [SetUp]
        public void Initialize()
        {
            _eventAggregatorMock = CreateEventAggregatorMock();
            Workspace = CreateSut();
            ScreenExtensions.TryActivate(Workspace);
            Workspace.AddSingletonVariable(new SingletonVariableBuilder().WithName("x")
                                                                         .WithDomain("1..10")
                                                                         .Inside(Workspace.WorkspaceModel.Model)
                                                                         .Build());
            Workspace.AddAggregateVariable(new AggregateVariableBuilder().WithName("y")
                                                                         .Inside(Workspace.WorkspaceModel.Model)
                                                                         .WithSize(2)
                                                                         .WithDomain("1..10")
                                                                         .Build());
            Workspace.AddExpressionConstraint(new ExpressionConstraintBuilder().WithExpression("$x > 1")
                                                                               .Inside(Workspace.WorkspaceModel.Model)
                                                                               .Build());
            Workspace.AddExpressionConstraint(new ExpressionConstraintBuilder().WithExpression("$y[0] <> $y[1]")
                                                                               .Inside(Workspace.WorkspaceModel.Model)
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
        public void SolveWithValidModelPublishesModelSolvedMessage()
        {
            Workspace.SolveModel();
            _eventAggregatorMock.Verify(_ => _.Publish(It.Is<ModelSolvedMessage>(msg => msg.Result.Status == SolveStatus.Success), It.IsAny<Action<System.Action>>()),
                                        Times.Once);
        }

        [Test]
        public void AddWithValidSingletonVariablePublishesVariableAddedMessage()
        {
            Workspace.AddSingletonVariable(new SingletonVariableBuilder().WithName("z")
                                                                         .Inside(Workspace.WorkspaceModel.Model)
                                                                         .Build());
            _eventAggregatorMock.Verify(_ => _.Publish(It.Is<SingletonVariableAddedMessage>(msg => msg.NewVariableName == "z"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        [Test]
        public void AddWithValidAggregatorVariablePublishesVariableAddedMessage()
        {
            Workspace.AddAggregateVariable(new AggregateVariableBuilder().WithName("z")
                                                                         .Inside(Workspace.WorkspaceModel.Model)
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

        [Test]
        public void DeleteWithValidVariableDeletesVariableFromModel()
        {
            var variableToDelete = Workspace.ModelEditor.GetVariableByName("x");
            Workspace.ModelEditor.DeleteVariable(variableToDelete);
            var deletedVariable = Workspace.WorkspaceModel.Model.GetVariableByName("x");
            Assert.That(deletedVariable, Is.Null, "Deleted variable should not exist any more.");
        }

        private WorkspaceViewModel CreateSut()
        {
            var workspaceViewModel = new WorkspaceViewModel(CreateDataService(),
                                                            CreateWindowManagerMock().Object,
                                                            _eventAggregatorMock.Object,
                                                            CreateViewModelFactoryMock().Object,
                                                            CreateModelValidator());

            return workspaceViewModel;
        }

        private ModelValidatorViewModel CreateModelValidator()
        {
            return new ModelValidatorViewModel(CreateWindowManagerMock().Object, CreateDataService());
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
            viewModelFactoryMock.Setup(factory => factory.CreateBundleEditor())
                                .Returns(new BundleEditorViewModel(new BundleModel(), CreateDataService(), CreateWindowManagerMock().Object, _eventAggregatorMock.Object));
            viewModelFactoryMock.Setup(_ => _.CreateModelEditor())
                                .Returns(new ModelEditorTabViewModel(CreateDataService(), _eventAggregatorMock.Object, CreateWindowManagerMock().Object));

            return viewModelFactoryMock;
        }

        private Mock<IEventAggregator> CreateEventAggregatorMock()
        {
            return new Mock<IEventAggregator>();
        }
    }
}
