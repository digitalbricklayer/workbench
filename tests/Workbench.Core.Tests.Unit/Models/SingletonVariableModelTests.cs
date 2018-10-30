using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class SingletonVariableModelTests
    {
        [Test]
        public void InitializeVariableWithValidNameSetsExpectedNameInVariable()
        {
            var sut = new SingletonVariableModel(CreateModel(), new ModelName("x"));
            Assert.That(sut.Name, Is.EqualTo(new ModelName("x")));
        }

        [Test]
        public void InitializeVariableWithEmptyExpressionWoutWhitespace()
        {
            var sut = new SingletonVariableModel(CreateModel(), new ModelName("x"), new InlineDomainModel());
            Assert.That(sut.DomainExpression.IsEmpty, Is.True);
        }

        [Test]
        public void InitializeVariableWithDomainReferenceRawExpressionWithWhitespace()
        {
            var sut = new SingletonVariableModel(CreateModel(), new ModelName("x"), new InlineDomainModel("   $A    "));
            Assert.That(sut.DomainExpression.DomainReference.DomainName.Name, Is.EqualTo("A"));
        }

        [Test]
        public void InitializeVariableWithDomainReferenceRawExpressionWoutWhitespace()
        {
            var sut = new SingletonVariableModel(CreateModel(), new ModelName("x"), new InlineDomainModel("$A"));
            Assert.That(sut.DomainExpression.DomainReference.DomainName.Name, Is.EqualTo("A"));
        }

        [Test]
        public void InitializeVariableWithInlineRawExpressionWoutWhitespace()
        {
            var sut = new SingletonVariableModel(CreateModel(), new ModelName("x"), new InlineDomainModel("1..10"));
            Assert.That(sut.DomainExpression.InlineDomain, Is.InstanceOf<RangeDomainExpressionNode>());
        }

        private static ModelModel CreateModel()
        {
            return new WorkspaceModel().Model;
        }
    }
}
