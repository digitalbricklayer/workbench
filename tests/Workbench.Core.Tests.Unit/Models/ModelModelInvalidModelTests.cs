using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class ModelModelInvalidModelTests
    {
        [Test]
        public void ValidateWithAnInvalidModelReturnsFalse()
        {
            var sut = MakeModelWithMissingVariable();
            var actualValidationResult = sut.Validate();
            Assert.That(actualValidationResult, Is.False);
        }

        [Test]
        public void ValidateWithAModelMissingVariablePopulatesErrors()
        {
            var sut = MakeModelWithMissingVariable();
            var validationContext = new ModelValidationContext();
            sut.Validate(validationContext);
            Assert.That(validationContext.Errors, Is.Not.Empty);
        }

        [Test]
        public void ValidateWithAModelMissingVariableHasErrrosIsTrue()
        {
            var sut = MakeModelWithMissingVariable();
            var validationContext = new ModelValidationContext();
            sut.Validate(validationContext);
            Assert.That(validationContext.HasErrors, Is.True);
        }

        [Test]
        public void ValidateWithAModelMissingSharedDomainPopulatesErrors()
        {
            var sut = MakeModelWithMissingSharedDomain();
            var validationContext = new ModelValidationContext();
            sut.Validate(validationContext);
            Assert.That(validationContext.Errors, Is.Not.Empty);
        }

        [Test]
        public void ValidateWithAModelMissingSharedDomainHasErrorsIsTrue()
        {
            var sut = MakeModelWithMissingSharedDomain();
            var validationContext = new ModelValidationContext();
            sut.Validate(validationContext);
            Assert.That(validationContext.HasErrors, Is.True);
        }

        private static ModelModel MakeModelWithMissingVariable()
        {
            var workspace = WorkspaceModel.Create("An invalid model")
                                          .AddSingleton("x", "1..9")
                                          .AddSingleton("y", "1..9")
                                          .WithConstraintExpression("x > z")
                                          .Build();

            return workspace.Model;
        }

        private static ModelModel MakeModelWithMissingSharedDomain()
        {
            var workspace = WorkspaceModel.Create("A model missing shared domain")
                                          .WithSharedDomain("a", "1..10")
                                          .AddSingleton("x", "b")
                                          .AddSingleton("y", "1..9")
                                          .WithConstraintExpression("x > y")
                                          .Build();

            return workspace.Model;
        }
    }
}
