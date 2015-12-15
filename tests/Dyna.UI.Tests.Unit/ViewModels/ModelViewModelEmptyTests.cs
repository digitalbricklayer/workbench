using System;
using Dyna.Core.Models;
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
            var modelViewModel = new ModelViewModel(new ModelModel());
            var variableViewModel = new VariableViewModel(new VariableModel("x"));
            modelViewModel.AddSingletonVariable(variableViewModel);
            variableViewModel.DomainExpression.Text = "1..10";
            var constraintViewModel = new ConstraintViewModel(new ConstraintModel("x", string.Empty));
            modelViewModel.AddConstraint(constraintViewModel);
            constraintViewModel.Expression.Text = "x > 1";

            return modelViewModel;
        }
    }
}
