using System;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class ExpressionConstraintModelTests
    {
        [Test]
        public void InitializeWithName()
        {
            var sut = new ExpressionConstraintModel("y", "");
            Assert.That(sut.Name, Is.EqualTo("y"));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Variable_Name_On_Left()
        {
            var sut = new ExpressionConstraintModel("x > 1");
            var leftVariableReference = (SingletonVariableReferenceNode) sut.Expression.Node.InnerExpression.LeftExpression.InnerExpression;
            Assert.That(leftVariableReference.VariableName, Is.EqualTo("x"));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Operator()
        {
            var sut = new ExpressionConstraintModel("     a1    >    999      ");
            Assert.That(sut.Expression.OperatorType, Is.EqualTo(OperatorType.Greater));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Literal_On_Right()
        {
            var sut = new ExpressionConstraintModel("y <= 44");
            var rightLiteral = (LiteralNode)sut.Expression.Node.InnerExpression.RightExpression.InnerExpression;
            Assert.That(rightLiteral.Value, Is.EqualTo(44));
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Variable_Name_On_Right()
        {
            var sut = new ExpressionConstraintModel("y = x");
            var rightVariableReference = (SingletonVariableReferenceNode)sut.Expression.Node.InnerExpression.RightExpression.InnerExpression;
            Assert.That(rightVariableReference.VariableName, Is.EqualTo("x"));
        }

        [Test]
        public void Initialize_With_Null_Expression_Throws_ArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new ExpressionConstraintModel((string)null));
        }

        [Test]
        public void InitializeWithRawExpressionParsesExpectedAggregateVariableNameOnLeft()
        {
            var sut = new ExpressionConstraintModel("xx[1] > 1");
            var leftVariableReference = (AggregateVariableReferenceNode)sut.Expression.Node.InnerExpression.LeftExpression.InnerExpression;
            Assert.That(leftVariableReference.VariableName, Is.EqualTo("xx"));
        }

        [Test]
        public void InitializeWithRawExpressionParsesExpectedAggregateVariableSubscriptOnLeft()
        {
            var sut = new ExpressionConstraintModel("xx[1] > 1");
            var leftVariableReference = (AggregateVariableReferenceNode)sut.Expression.Node.InnerExpression.LeftExpression.InnerExpression;
            Assert.That(leftVariableReference.SubscriptStatement.Subscript, Is.EqualTo(1));
        }

        [Test]
        public void InitializeWithRawExpressionParsesExpectedAggregateVariableNameOnRight()
        {
            var sut = new ExpressionConstraintModel("x[1] > x[2]");
            var rightVariableReference = (AggregateVariableReferenceNode)sut.Expression.Node.InnerExpression.RightExpression.InnerExpression;
            Assert.That(rightVariableReference.VariableName, Is.EqualTo("x"));
        }
    }
}
