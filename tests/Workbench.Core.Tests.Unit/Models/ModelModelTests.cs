using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
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
            var workspace = WorkspaceModel.Create("A valid model")
                                          .AddSingleton("x", "1..9")
                                          .AddSingleton("y", "1..9")
                                          .WithConstraintExpression("x > y")
                                          .Build();

            return workspace.Model;
        }
    }
}
