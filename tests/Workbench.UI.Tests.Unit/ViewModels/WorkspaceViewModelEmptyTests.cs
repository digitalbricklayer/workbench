using Caliburn.Micro;
using Workbench.ViewModels;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class WorkspaceViewModelEmptyTests
    {
        [Test]
        public void SolveWithValidModelReturnsSuccessStatus()
        {
            var sut = CreateValidWorkspace();
            var actualStatus = sut.SolveModel();
            Assert.That(actualStatus.IsSuccess, Is.True);
        }

        private static WorkspaceViewModel CreateValidWorkspace()
        {
            var workspaceViewModel = new WorkspaceViewModel(CreateDataService(),
                                                            CreateWindowManager(),
                                                            CreateEventAggregator(),
                                                            CreateViewModelService(),
                                                            CreateViewModelFactory());
            var variableViewModel = new VariableViewModel(new VariableModel("x"),
                                                          Mock.Of<IEventAggregator>());
            workspaceViewModel.Model.AddSingletonVariable(variableViewModel);
            variableViewModel.DomainExpression.Text = "1..10";
            var constraintViewModel = new ConstraintViewModel(new ExpressionConstraintModel("x", string.Empty));
            workspaceViewModel.Model.AddConstraint(constraintViewModel);
            constraintViewModel.Expression.Text = "x > 1";

            return workspaceViewModel;
        }

        private static IDataService CreateDataService()
        {
            return new DataService(Mock.Of<IWorkspaceReaderWriter>());
        }

        private static IViewModelFactory CreateViewModelFactory()
        {
            return new ViewModelFactory(CreateEventAggregator(), CreateWindowManager());
        }

        private static IViewModelService CreateViewModelService()
        {
            return new ViewModelService(CreateViewModelFactory());
        }

        private static IEventAggregator CreateEventAggregator()
        {
            return new Mock<IEventAggregator>().Object;
        }

        private static IWindowManager CreateWindowManager()
        {
            return new Mock<IWindowManager>().Object;
        }
    }
}
