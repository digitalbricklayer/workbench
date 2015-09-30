using Dyna.Core.Models;
using NUnit.Framework;

namespace Dyna.Core.Tests.Unit.Models
{
    [TestFixture]
    public class VariableModelTests
    {
        [Test]
        public void InitializeWithName()
        {
            var sut = new VariableModel("x");
            Assert.That(sut.Name, Is.EqualTo("x"));
        }

        [Test]
        public void InitializeWithEmptyExpressionWoutWhitespace()
        {
            var sut = new VariableModel("x", "");
            Assert.That(sut.DomainExpression.IsEmpty, Is.True);
        }

        [Test]
        public void InitializeWithDomainReferenceRawExpressionWithWhitespace()
        {
            var sut = new VariableModel("x", "   A    ");
            Assert.That(sut.DomainExpression.DomainReference.DomainName, Is.EqualTo("A"));
        }

        [Test]
        public void InitializeWithDomainReferenceRawExpressionWoutWhitespace()
        {
            var sut = new VariableModel("x", "A");
            Assert.That(sut.DomainExpression.DomainReference.DomainName, Is.EqualTo("A"));
        }

        [Test]
        public void InitializeWithInlineRawExpressionWoutWhitespace()
        {
            var sut = new VariableModel("x", "1..10");
            Assert.That(sut.DomainExpression.InlineDomain.Size, Is.EqualTo(10));
        }
    }
}
