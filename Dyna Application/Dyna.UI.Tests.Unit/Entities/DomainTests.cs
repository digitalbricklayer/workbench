using System.Linq;
using DynaApp.Entities;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.Entities
{
    [TestFixture]
    public class DomainTests
    {
        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Upper_Band()
        {
            // Arrange

            // Act
            var sut = new Domain("    1..9     ");

            // Assert
            Assert.That(sut.Expression.UpperBand, Is.EqualTo(9));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Lower_Band()
        {
            // Arrange

            // Act
            var sut = new Domain("    1..9     ");

            // Assert
            Assert.That(sut.Expression.LowerBand, Is.EqualTo(1));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_First_Value()
        {
            // Arrange

            // Act
            var sut = new Domain("    1..9     ");

            // Assert
            Assert.That(sut.Values.First(), Is.EqualTo(1));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Last_Value()
        {
            // Arrange

            // Act
            var sut = new Domain("    33..40     ");

            // Assert
            Assert.That(sut.Values.Last(), Is.EqualTo(40));
        }
    }
}
