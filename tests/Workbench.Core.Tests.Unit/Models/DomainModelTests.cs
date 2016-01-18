using System.Windows;
using System.Linq;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class DomainModelTests
    {
        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Upper_Band()
        {
            var sut = new DomainModel("A domain", new Point(0, 0), new DomainExpressionModel("    1..9     "));
            Assert.That(sut.Expression.UpperBand, Is.EqualTo(9));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Lower_Band()
        {
            var sut = new DomainModel("    1..9     ");
            Assert.That(sut.Expression.LowerBand, Is.EqualTo(1));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_First_Value()
        {
            var sut = new DomainModel("    1..9     ");
            Assert.That(sut.Values.First(), Is.EqualTo(1));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Last_Value()
        {
            var sut = new DomainModel("    33..40     ");
            Assert.That(sut.Values.Last(), Is.EqualTo(40));
        }
    }
}
