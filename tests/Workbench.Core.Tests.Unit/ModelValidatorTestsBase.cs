using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    public abstract class ModelValidatorTestsBase
    {
        [Test]
        public void ValidateReturnsFalse()
        {
            var makeModelWithMissingVariable = CreateModel();
            var actualValidationResult = new ModelValidator(makeModelWithMissingVariable).Validate();
            Assert.That(actualValidationResult, Is.False);
        }

        [Test]
        public void ValidatePopulatesContextErrors()
        {
            var sut = CreateModel();
            var validationContext = new ModelValidationContext();
            new ModelValidator(sut).Validate(validationContext);
            Assert.That(validationContext.Errors, Is.Not.Empty);
        }

        [Test]
        public void ValidateSetsContextHasErrorsIsTrue()
        {
            var sut = CreateModel();
            var validationContext = new ModelValidationContext();
            new ModelValidator(sut).Validate(validationContext);
            Assert.That(validationContext.HasErrors, Is.True);
        }

        protected abstract ModelModel CreateModel();
    }
}