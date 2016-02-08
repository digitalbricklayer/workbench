using System;
using Caliburn.Micro;
using Workbench.ViewModels;
using Moq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Messages;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ModelViewModelTests
    {
        private readonly Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();

        [Test]
        public void SolveWithValidModelReturnsSuccessStatus()
        {
            var sut = CreateValidModel();
            var actualStatus = sut.Solve();
            Assert.That(actualStatus.IsSuccess, Is.True);
        }

        [Test]
        public void DeleteWithValidVariablePublishesVariableDeletedMessage()
        {
            var sut = CreateValidModel();
            var variableToDelete = sut.GetVariableByName("x");
            sut.DeleteVariable(variableToDelete);
            this.eventAggregatorMock.Verify(_ => _.Publish(It.Is<VariableDeletedMessage>(msg => msg.VariableName == "x"), It.IsAny<Action<System.Action>>()),
                                            Times.Once);
        }

        private ModelViewModel CreateValidModel()
        {
            var modelViewModel = new ModelViewModel(new ModelModel(),
                                                    CreateWindowManager(),
                                                    this.eventAggregatorMock.Object);
            modelViewModel.AddSingletonVariable(new VariableViewModel(new VariableModel("x", new VariableDomainExpressionModel("1..10")), Mock.Of<IEventAggregator>()));
            modelViewModel.AddAggregateVariable(new AggregateVariableViewModel(new AggregateVariableModel("y", 2, new VariableDomainExpressionModel("1..10")), Mock.Of<IEventAggregator>()));
            modelViewModel.AddConstraint(new ConstraintViewModel(new ConstraintModel("x", "x > 1")));
            modelViewModel.AddConstraint(new ConstraintViewModel(new ConstraintModel("aggregates must be different",
                                                                                     "y[1] <> y[2]")));

            return modelViewModel;
        }

        private static IWindowManager CreateWindowManager()
        {
            return new Mock<IWindowManager>().Object;
        }
    }
}
