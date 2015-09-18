using Dyna.Core.Models;
using NUnit.Framework;

namespace Dyna.Core.Tests.Unit.Models
{
    [TestFixture]
    public class ModelModelTests
    {
        [Test]
        public void An_Empty_Model_Is_A_Valid_Model()
        {
            var sut = new ModelModel("An Empty Model");
            var actualValidationResult = sut.Validate();
            Assert.That(actualValidationResult, Is.True);
        }

        [Test]
        public void Validate_With_A_Valid_Model_Returns_True()
        {
            var sut = MakeValidModel();
            var actualValidationResult = sut.Validate();
            Assert.That(actualValidationResult, Is.True);
        }

        [Test]
        public void Validate_With_A_Valid_Model_Does_Not_Populate_Errors()
        {
            var sut = MakeValidModel();
            sut.Validate();
            Assert.That(sut.Errors, Is.Empty);
        }

        [Test]
        public void Validate_With_An_Invalid_Model_Returns_False()
        {
            var sut = MakeModelWithMissingVariable();
            var actualValidationResult = sut.Validate();
            Assert.That(actualValidationResult, Is.False);
        }

        [Test]
        public void Validate_With_A_Model_Missing_Variable_Populates_Errors()
        {
            var sut = MakeModelWithMissingVariable();
            sut.Validate();
            Assert.That(sut.Errors, Is.Not.Empty);
        }

        [Test]
        public void Validate_With_A_Model_Missing_Shared_Domain_Populates_Errors()
        {
            var sut = MakeModelWithMissingSharedDomain();
            sut.Validate();
            Assert.That(sut.Errors, Is.Not.Empty);
        }

        private static ModelModel MakeValidModel()
        {
            return ModelModel.Create("A valid model")
                             .AddVariable("x", new DomainModel("1..9"))
                             .AddVariable("y", new DomainModel("1..9"))
                             .WithConstraint("x > y")
                             .Build();
        }

        private static ModelModel MakeModelWithMissingVariable()
        {
            return ModelModel.Create("An invalid model")
                             .AddVariable("x", new DomainModel("1..9"))
                             .AddVariable("y", new DomainModel("1..9"))
                             .WithConstraint("x > z")
                             .Build();
        }

        private static ModelModel MakeModelWithMissingSharedDomain()
        {
            return ModelModel.Create("An model missing shared domain")
                             .WithSharedDomain("a", "1..10")
                             .AddVariable("x", "b")
                             .AddVariable("y", new DomainModel("1..9"))
                             .WithConstraint("x > y")
                             .Build();
        }
    }
}
