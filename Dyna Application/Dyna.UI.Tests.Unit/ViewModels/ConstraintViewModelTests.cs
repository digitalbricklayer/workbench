using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class ConstraintViewModelTests
    {
        [Test]
        public void Constraint_Cannot_Connect_To_Another_Constraint()
        {
            // Arrange
            var sut = new ConstraintViewModel("X");

            // Act
            var actualResult = sut.IsConnectableTo(new ConstraintViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.False);
        }

        [Test]
        public void Constraint_Cannot_Connect_To_A_Variable()
        {
            // Arrange
            var sut = new ConstraintViewModel("X");

            // Act
            var actualResult = sut.IsConnectableTo(new VariableViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.False);
        }

        [Test]
        public void Constraint_Cannot_Connect_To_A_Domain()
        {
            // Arrange
            var sut = new ConstraintViewModel("X");

            // Act
            var actualResult = sut.IsConnectableTo(new DomainViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.False);
        }
    }
}
