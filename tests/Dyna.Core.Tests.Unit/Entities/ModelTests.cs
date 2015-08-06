using Dyna.Core.Entities;
using NUnit.Framework;

namespace Dyna.Core.Tests.Unit.Entities
{
    [TestFixture]
    public class ModelTests
    {
        [Test]
        public void An_Empty_Model_Is_A_Valid_Model()
        {
            var sut = new Model("An Empty Model");
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

        private static Model MakeValidModel()
        {
            return Model.Create("A valid model")
                        .AddVariable("x", new Domain("1..9"))
                        .AddVariable("y", new Domain("1..9"))
                        .WithConstraint("x > y")
                        .Build();
        }

        private static Model MakeModelWithMissingVariable()
        {
            return Model.Create("An invalid model")
                        .AddVariable("x", new Domain("1..9"))
                        .AddVariable("y", new Domain("1..9"))
                        .WithConstraint("x > z")
                        .Build();
        }

        private static Model MakeModelWithMissingSharedDomain()
        {
            return Model.Create("An model missing shared domain")
                        .WithSharedDomain("a", "1..10")
                        .AddVariable("x", "b")
                        .AddVariable("y", new Domain("1..9"))
                        .WithConstraint("x > y")
                        .Build();
        }
    }
}
