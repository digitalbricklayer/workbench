using Workbench.ViewModels;
using NUnit.Framework;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class DomainExpressionViewModelTests
    {
        [Test]
        public void Initialize_With_Default_Values_Text_Is_Empty()
        {
            var sut = new DomainExpressionViewModel();
            Assert.That(sut.Text, Is.Empty);
        }
    }
}
