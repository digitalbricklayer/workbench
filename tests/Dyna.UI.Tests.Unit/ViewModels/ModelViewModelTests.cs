using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ModelViewModelTests
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
            modelViewModel.AddSingletonVariable(new VariableViewModel("x", new VariableDomainExpressionViewModel("1..10")));
            modelViewModel.AddAggregateVariable(new AggregateVariableViewModel("x", 2, new VariableDomainExpressionViewModel("1..10")));
            modelViewModel.AddConstraint(new ConstraintViewModel("x", "x > 1"));
            modelViewModel.AddConstraint(new ConstraintViewModel("aggregates must be different",
                                                                 "x[1] <> x[2]"));

            return modelViewModel;
        }
    }
}
