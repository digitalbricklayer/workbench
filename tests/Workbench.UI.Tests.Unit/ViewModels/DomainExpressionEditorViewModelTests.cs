using Workbench.ViewModels;
using NUnit.Framework;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class DomainExpressionEditorViewModelTests
    {
        [Test]
        public void Initialize_With_Default_Values_Text_Is_Empty()
        {
            var sut = new DomainExpressionEditorViewModel();
            Assert.That(sut.Text, Is.Empty);
        }
    }
}
