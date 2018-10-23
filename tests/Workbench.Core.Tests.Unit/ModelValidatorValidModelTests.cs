using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    [TestFixture]
    public class ModelValidatorValidModelTests
    {
        [Test]
        public void ValidateWithAValidModelReturnsTrue()
        {
            var sut = MakeValidModel();
            var actualValidationResult = new ModelValidator(sut).Validate();
            Assert.That(actualValidationResult, Is.True);
        }

        [Test]
        public void ValidateWithAValidModelDoesNotPopulateErrors()
        {
            var sut = MakeValidModel();
            var validationContext = new ModelValidationContext();
            new ModelValidator(sut).Validate(validationContext);
            Assert.That(validationContext.Errors, Is.Empty);
        }

        [Test]
        public void ValidateWithAValidModelHasErrorsReturnsTrue()
        {
            var sut = MakeValidModel();
            var validationContext = new ModelValidationContext();
            new ModelValidator(sut).Validate(validationContext);
            Assert.That(validationContext.HasErrors, Is.False);
        }

        private static ModelModel MakeValidModel()
        {
            var workspace = new WorkspaceBuilder("A valid model")
                .AddSingleton("x", "1..9")
                .AddAggregate("y", 10, "1..9")
                .WithConstraintExpression("x > y")
                .Build();

            return workspace.Model;
        }
    }
}
