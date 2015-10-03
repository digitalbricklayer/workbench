using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class VariableViewModelTests
    {
        [Test]
        public void UpdateVariableDomainExpressionWithDomainReferenceUpdatesModel()
        {
            var sut = CreateVariable();
            sut.DomainExpression.Text = "x";
            Assert.That(sut.DomainExpression.Model.DomainReference.DomainName, Is.EqualTo("x"));
        }

        [Test]
        public void UpdateDomainExpressionWithInlineDomainUpdatesModel()
        {
            var sut = CreateVariable();
            sut.DomainExpression.Text = "1..10";
            Assert.That(sut.DomainExpression.Model.InlineDomain.Size, Is.EqualTo(10));
        }

        private static VariableViewModel CreateVariable()
        {
            return new VariableViewModel("X");
        }
    }
}
