using System.Linq;
using Dyna.Core.Models;
using NUnit.Framework;

namespace Dyna.Core.Tests.Unit.Models
{
    [TestFixture]
    public class DomainModelTests
    {
        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Upper_Band()
        {
            // Arrange

            // Act
            var sut = new DomainModel("A domain", "    1..9     ");

            // Assert
            Assert.That((object) sut.Expression.UpperBand, Is.EqualTo(9));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Lower_Band()
        {
            // Arrange

            // Act
            var sut = new DomainModel("    1..9     ");

            // Assert
            Assert.That((object) sut.Expression.LowerBand, Is.EqualTo(1));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_First_Value()
        {
            // Arrange

            // Act
            var sut = new DomainModel("    1..9     ");

            // Assert
            Assert.That(Enumerable.First<int>(sut.Values), Is.EqualTo(1));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Last_Value()
        {
            // Arrange

            // Act
            var sut = new DomainModel("    33..40     ");

            // Assert
            Assert.That(Enumerable.Last<int>(sut.Values), Is.EqualTo(40));
        }
    }
}
