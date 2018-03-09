using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class ModelModelTests
    {
        [Test]
        public void AddSingletonVariableToModelVariableAddedToCorrectModel()
        {
            var sut = MakeValidModel();
            var xVariable = sut.GetVariableByName("x");
            Assert.That(xVariable.Model, Is.SameAs(sut));
        }

        [Test]
        public void AddAggregateVariableToModelVariableAddedToCorrectModel()
        {
            var sut = MakeValidModel();
            var xVariable = sut.GetVariableByName("y");
            Assert.That(xVariable.Model, Is.SameAs(sut));
        }

        [Test]
        public void ValidateWithAValidModelReturnsTrue()
        {
            var sut = MakeValidModel();
            var actualValidationResult = sut.Validate();
            Assert.That(actualValidationResult, Is.True);
        }

        [Test]
        public void ValidateWithAValidModelDoesNotPopulateErrors()
        {
            var sut = MakeValidModel();
            var validationContext = new ModelValidationContext();
            sut.Validate(validationContext);
            Assert.That(validationContext.Errors, Is.Empty);
        }

        [Test]
        public void ValidateWithAValidModelHasErrorsReturnsTrue()
        {
            var sut = MakeValidModel();
            var validationContext = new ModelValidationContext();
            sut.Validate(validationContext);
            Assert.That(validationContext.HasErrors, Is.False);
        }

        private static ModelModel MakeValidModel()
        {
            var workspace = WorkspaceModel.Create("A valid model")
                                          .AddSingleton("x", "1..9")
                                          .AddAggregate("y", 10, "1..9")
                                          .WithConstraintExpression("x > y")
                                          .Build();

            return workspace.Model;
        }
    }
}
