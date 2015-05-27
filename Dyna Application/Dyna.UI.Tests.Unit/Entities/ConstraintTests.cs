using DynaApp.Entities;
using NUnit.Framework;

namespace DynaApp.UI.Tests.Unit.Domain
{
    [TestFixture]
    public class ConstraintTests
    {
        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Variable_Name_On_Left()
        {
            // Arrange

            // Act
            var sut = new Constraint("x > 1");

            // Assert
            Assert.That(sut.Expression.Left.Name, Is.EqualTo("x"));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Operator()
        {
            // Arrange

            // Act
            var sut = new Constraint("a1 > 999");

            // Assert
            Assert.That(sut.Expression.OperatorType, Is.EqualTo(OperatorType.Greater));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Literal_On_Right()
        {
            // Arrange

            // Act
            var sut = new Constraint("y <= 44");

            // Assert
            Assert.That(sut.Expression.Right.Literal.Value, Is.EqualTo(44));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Variable_Name_On_Right()
        {
            // Arrange

            // Act
            var sut = new Constraint("y = x");

            // Assert
            Assert.That(sut.Expression.Right.Variable.Name, Is.EqualTo("x"));
        }
    }
}
