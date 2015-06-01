using DynaApp.Entities;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.Entities
{
    [TestFixture]
    public class ModelTests
    {
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
        public void Validate_With_An_Invalid_Model_Populates_Errors()
        {
            var sut = MakeModelWithMissingVariable();
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
    }
}
