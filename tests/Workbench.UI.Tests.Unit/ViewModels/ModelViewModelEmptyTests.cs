using Caliburn.Micro;
using Workbench.ViewModels;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ModelViewModelEmptyTests
    {
        [Test]
        public void SolveWithValidModelReturnsSuccessStatus()
        {
            var sut = CreateValidModel();
            var actualStatus = sut.Solve();
            Assert.That(actualStatus.IsSuccess, Is.True);
        }

        private static ModelViewModel CreateValidModel()
        {
            var modelViewModel = new ModelViewModel(new ModelModel(),
                                                    CreateWindowManager(),
                                                    CreateEventAggregator());
            var variableViewModel = new VariableViewModel(new VariableModel("x"),
                                                          Mock.Of<IEventAggregator>());
            modelViewModel.AddSingletonVariable(variableViewModel);
            variableViewModel.DomainExpression.Text = "1..10";
            var constraintViewModel = new ConstraintViewModel(new ConstraintModel("x", string.Empty));
            modelViewModel.AddConstraint(constraintViewModel);
            constraintViewModel.Expression.Text = "x > 1";

            return modelViewModel;
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
