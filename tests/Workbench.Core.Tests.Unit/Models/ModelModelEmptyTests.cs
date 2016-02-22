using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class ModelModelEmptyTests
    {
        [Test]
        public void AnEmptyModelIsAValidModel()
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
