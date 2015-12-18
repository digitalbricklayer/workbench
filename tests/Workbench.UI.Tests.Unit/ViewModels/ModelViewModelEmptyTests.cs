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
            var modelViewModel = new ModelViewModel(new ModelModel(), CreateWindowManager());
            var variableViewModel = new VariableViewModel(new VariableModel("x"));
            modelViewModel.AddSingletonVariable(variableViewModel);
            variableViewModel.DomainExpression.Text = "1..10";
            var constraintViewModel = new ConstraintViewModel(new ConstraintModel("x", string.Empty));
            modelViewModel.AddConstraint(constraintViewModel);
            constraintViewModel.Expression.Text = "x > 1";

            return modelViewModel;
        }

        private static IWindowManager CreateWindowManager()
        {
            return new Mock<IWindowManager>().Object;
        }
    }
}
