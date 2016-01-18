using System;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class ConstraintModelTests
    {
        [Test]
        public void InitializeWithName()
        {
            var sut = new ConstraintModel("y", "");
            Assert.That(sut.Name, Is.EqualTo("y"));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Variable_Name_On_Left()
        {
            var sut = new ConstraintModel("x > 1");
            Assert.That(sut.Expression.Left.Variable.Name, Is.EqualTo("x"));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Operator()
        {
            var sut = new ConstraintModel("     a1    >    999      ");
            Assert.That(sut.Expression.OperatorType, Is.EqualTo(OperatorType.Greater));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Literal_On_Right()
        {
            var sut = new ConstraintModel("y <= 44");
            Assert.That(sut.Expression.Right.Literal.Value, Is.EqualTo(44));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Variable_Name_On_Right()
        {
            var sut = new ConstraintModel("y = x");
            Assert.That(sut.Expression.Right.Variable.Name, Is.EqualTo("x"));
        }

        [Test]
        public void Initialize_With_Empty_Expression_Throws_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ConstraintModel(string.Empty));
        }

        [Test]
        public void InitializeWithRawExpressionParsesExpectedAggregateVariableNameOnLeft()
        {
            var sut = new ConstraintModel("xx[1] > 1");
            Assert.That(sut.Expression.Left.AggregateReference.IdentifierName, Is.EqualTo("xx"));
        }

        [Test]
        public void InitializeWithRawExpressionParsesExpectedAggregateVariableSubscriptOnLeft()
        {
            var sut = new ConstraintModel("xx[1] > 1");
            Assert.That(sut.Expression.Left.AggregateReference.Index, Is.EqualTo(1));
        }

        [Test]
        public void InitializeWithRawExpressionParsesExpectedAggregateVariableNameOnRight()
        {
            var sut = new ConstraintModel("x[1] > x[2]");
            Assert.That(sut.Expression.Right.AggregateReference.IdentifierName, Is.EqualTo("x"));
        }
    }
}
