using Caliburn.Micro;
using Workbench.ViewModels;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ModelViewModelTests
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
            modelViewModel.AddSingletonVariable(new VariableViewModel(new VariableModel("x", new VariableDomainExpressionModel("1..10"))));
            modelViewModel.AddAggregateVariable(new AggregateVariableViewModel(new AggregateVariableModel("y", 2, new VariableDomainExpressionModel("1..10"))));
            modelViewModel.AddConstraint(new ConstraintViewModel(new ConstraintModel("x", "x > 1")));
            modelViewModel.AddConstraint(new ConstraintViewModel(new ConstraintModel("aggregates must be different",
                                                                                     "y[1] <> y[2]")));

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
