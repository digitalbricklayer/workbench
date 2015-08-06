using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class DomainViewModelTests
    {
        [Test]
        public void Domain_Cannot_Connect_To_Another_Domain()
        {
            // Arrange
            var sut = new DomainViewModel("X");

            // Act
            var actualResult = sut.IsConnectableTo(new DomainViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.False);
        }

        [Test]
        public void Domain_Cannot_Connect_To_A_Variable()
        {
            // Arrange
            var sut = new DomainViewModel("X");

            // Act
            var actualResult = sut.IsConnectableTo(new VariableViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.False);
        }

        [Test]
        public void Domain_Cannot_Connect_To_A_Constraint()
        {
            // Arrange
            var sut = new DomainViewModel("X");

            // Act
            var actualResult = sut.IsConnectableTo(new ConstraintViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.False);
        }

        [Test]
        public void IsValid_With_Empty_Expression_Returns_False()
        {
            var sut = new DomainViewModel("X");
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void IsValid_With_Valid_Expression_Returns_True()
        {
            var sut = new DomainViewModel("X", "1..2");
            Assert.That(sut.IsValid, Is.True);
        }
    }
}
