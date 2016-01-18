using Workbench.ViewModels;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ConstraintExpressionViewModelTests
    {
        [Test]
        public void Initialize_With_Default_Values_Text_Is_Empty()
        {
            var sut = new ConstraintExpressionViewModel(new ConstraintExpressionModel());
            Assert.That(sut.Text, Is.Empty);
        }
    }
}
