using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class ModelModelTests
    {
        [Test]
        public void AddSingletonVariableToModelVariableAddedToCorrectModel()
        {
            var sut = MakeValidModel();
            var xVariable = sut.GetVariableByName("x");
            Assert.That(xVariable.Parent, Is.SameAs(sut));
        }

        [Test]
        public void AddAggregateVariableToModelVariableAddedToCorrectModel()
        {
            var sut = MakeValidModel();
            var xVariable = sut.GetVariableByName("y");
            Assert.That(xVariable.Parent, Is.SameAs(sut));
        }

        [Test]
        public void FindRootReturnsModel()
        {
            var sut = MakeValidModel();
            var actualRoot = sut.FindRoot();
            Assert.That(actualRoot, Is.SameAs(sut));
        }

        private static ModelModel MakeValidModel()
        {
            var workspace = new WorkspaceBuilder("A valid model")
                                          .AddSingleton("x", "1..9")
                                          .AddAggregate("y", 10, "1..9")
                                          .WithConstraintExpression("x > y")
                                          .Build();

            return workspace.Model;
        }
    }
}
