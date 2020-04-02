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
            var actualValidationResult = new ModelValidator(MakeEmptyWorkspace().Model).Validate();
            Assert.That(actualValidationResult, Is.True);
        }

        private static WorkspaceModel MakeEmptyWorkspace()
        {
            return new WorkspaceModel(new ModelName("An Empty Model"));
        }
    }
}
