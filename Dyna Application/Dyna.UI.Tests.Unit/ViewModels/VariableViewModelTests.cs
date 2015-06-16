using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class VariableViewModelTests
    {
        [Test]
        public void Variable_Cannot_Connect_To_Another_Variable()
        {
            // Arrange
            var sut = new VariableViewModel("X");

            // Act
            var actualResult= sut.IsConnectableTo(new VariableViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.False);
        }

        [Test]
        public void Variable_Can_Connect_To_A_Constraint()
        {
            // Arrange
            var sut = new VariableViewModel("X");

            // Act
            var actualResult = sut.IsConnectableTo(new ConstraintViewModel("Y"));

            // Assert
            Assert.That(actualResult, Is.True);
        }

        [Test]
        public void Variable_Can_Connect_To_A_Domain()
        {
            // Arrange
            var sut = new VariableViewModel("X");

            // Act
            var actualResult = sut.IsConnectableTo(new DomainViewModel("Z"));

            // Assert
            Assert.That(actualResult, Is.True);
        }
    }
}
