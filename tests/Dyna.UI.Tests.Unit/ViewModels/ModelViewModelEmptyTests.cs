using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ModelViewModelEmptyTests
    {
        [Test]
        public void SolveWithValidModelReturnsSuccessStatus()
        {
            var sut = CreateValidModel();
            var actualStatus = sut.Solve(null);
            Assert.That(actualStatus.IsSuccess, Is.True);
        }

        private static ModelViewModel CreateValidModel()
        {
            var modelViewModel = new ModelViewModel();
            var variableViewModel = new VariableViewModel("x");
            modelViewModel.AddVariable(variableViewModel);
            variableViewModel.DomainExpression.Text = "1..10";
            var constraintViewModel = new ConstraintViewModel("x");
            modelViewModel.AddConstraint(constraintViewModel);
            constraintViewModel.Expression.Text = "x > 1";

            return modelViewModel;
        }
    }
}
