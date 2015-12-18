using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class VariableModelTests
    {
        [Test]
        public void InitializeVariableWithValidNameSetsExpectedNameInVariable()
        {
            var sut = new VariableModel("x");
            Assert.That(sut.Name, Is.EqualTo("x"));
        }

        [Test]
        public void InitializeVariableWithEmptyExpressionWoutWhitespace()
        {
            var sut = new VariableModel("x", "");
            Assert.That(sut.DomainExpression.IsEmpty, Is.True);
        }

        [Test]
        public void InitializeVariableWithDomainReferenceRawExpressionWithWhitespace()
        {
            var sut = new VariableModel("x", "   A    ");
            Assert.That(sut.DomainExpression.DomainReference.DomainName, Is.EqualTo("A"));
        }

        [Test]
        public void InitializeVariableWithDomainReferenceRawExpressionWoutWhitespace()
        {
            var sut = new VariableModel("x", "A");
            Assert.That(sut.DomainExpression.DomainReference.DomainName, Is.EqualTo("A"));
        }

        [Test]
        public void InitializeVariableWithInlineRawExpressionWoutWhitespace()
        {
            var sut = new VariableModel("x", "1..10");
            Assert.That(sut.DomainExpression.InlineDomain.Size, Is.EqualTo(10));
        }
    }
}
