using Dyna.Core.Models;
using NUnit.Framework;

namespace Dyna.Core.Tests.Unit.Models
{
    [TestFixture]
    public class ModelModelEmptyTests
    {
        [Test]
        public void An_Empty_Model_Is_A_Valid_Model()
        {
            var sut = MakeEmptyModel();
            var actualValidationResult = sut.Validate();
            Assert.That(actualValidationResult, Is.True);
        }

        private static ModelModel MakeEmptyModel()
        {
            return new ModelModel("An Empty Model");
        }
    }
}
