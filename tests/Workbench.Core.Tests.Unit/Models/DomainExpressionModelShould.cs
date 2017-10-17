using NUnit.Framework;
using Workbench.Core.Grammars;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class DomainExpressionModelShould
    {
        [Test]
        public void ParseExpressionWithRhsNumberLiteral()
        {
            var sut = new DomainExpressionModel("    1..9     ");
            Assert.That(sut.Node.RightExpression.Inner, Is.InstanceOf<NumberLiteralNode>());
            var rightBandLiteral = (NumberLiteralNode) sut.Node.RightExpression.Inner;
            Assert.That(rightBandLiteral.Value, Is.EqualTo(9));
        }

        [Test]
        public void ParseExpressionWithLhsNumberLiteral()
        {
            var sut = new DomainExpressionModel("    1..9     ");
            Assert.That(sut.Node.LeftExpression.Inner, Is.InstanceOf<NumberLiteralNode>());
            var leftBandLiteral = (NumberLiteralNode) sut.Node.LeftExpression.Inner;
            Assert.That(leftBandLiteral.Value, Is.EqualTo(1));
        }

        [Test]
        public void ParseExpressionWithSizeFunctionCall()
        {
            var sut = new DomainExpressionModel("1..size(x)");
            Assert.That(sut.Node.RightExpression.Inner, Is.InstanceOf(typeof(FunctionInvocationNode)));
        }
    }
}
