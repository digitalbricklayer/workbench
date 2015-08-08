using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ConstraintExpressionViewModelTests
    {
        [Test]
        public void Initialize_With_Default_Values_Text_Is_Empty()
        {
            var sut = new ConstraintExpressionViewModel();
            Assert.That(sut.Expression, Is.Empty);
        }
    }
}
