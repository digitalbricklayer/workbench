using Dyna.Core.Models;
using NUnit.Framework;

namespace Dyna.Core.Tests.Unit.Models
{
    [TestFixture]
    public class ModelModelTests
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

        private static ModelModel MakeValidModel()
        {
            return ModelModel.Create("A valid model")
                             .AddVariable("x", "1..9")
                             .AddVariable("y", "1..9")
                             .WithConstraint("x > y")
                             .Build();
        }
    }
}
