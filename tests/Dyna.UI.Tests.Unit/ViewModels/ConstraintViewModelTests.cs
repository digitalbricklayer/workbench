using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ConstraintViewModelTests
    {
        [Test]
        public void IsValid_With_Empty_Expression_Returns_False()
        {
            var sut = new ConstraintViewModel("X");
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void IsValid_With_Valid_Expression_Returns_True()
        {
            var sut = new ConstraintViewModel("X", "X < Y");
            Assert.That(sut.IsValid, Is.True);
        }

        [Test]
        public void UpdateConstraintExpressionTextUpdatesExpressionModel()
        {
            var sut = new ConstraintViewModel();
            sut.Expression.Text = "x > 1";
            Assert.That(sut.Expression.Model.Left.Variable.Name, Is.EqualTo("x"));
        }

        [Test]
        public void UpdateConstraintExpressionTextUpdatesConstraintModel()
        {
            var sut = new ConstraintViewModel();
            sut.Expression.Text = "x > 1";
            Assert.That(sut.Model.Expression.Left.Variable.Name, Is.EqualTo(sut.Expression.Model.Left.Variable.Name));
        }
    }
}
