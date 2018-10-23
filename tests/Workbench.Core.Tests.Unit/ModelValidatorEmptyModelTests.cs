using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    [TestFixture]
    public class ModelValidatorEmptyModelTests
    {
        [Test]
        public void AnEmptyModelIsAValidModel()
        {
            var sut = MakeEmptyModel();
            var actualValidationResult = new ModelValidator(sut).Validate();
            Assert.That(actualValidationResult, Is.True);
        }

        private static ModelModel MakeEmptyModel()
        {
            return new ModelModel(new ModelName("An Empty Model"));
        }
    }
}
