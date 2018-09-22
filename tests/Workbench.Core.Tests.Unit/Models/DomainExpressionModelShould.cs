using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class DomainExpressionModelShould
    {
        [Test]
        public void ParseExpressionWithRangeInnerIsRangeNode()
        {
            var sut = new DomainExpressionModel("    1..9     ");
            Assert.That(sut.Node.Inner, Is.InstanceOf<RangeDomainExpressionNode>());
        }

        [Test]
        public void ParseExpressionWithRhsNumberLiteral()
        {
            var sut = new DomainExpressionModel("    1..9     ");
            var rangeExpressionNode = (RangeDomainExpressionNode)sut.Node.Inner;
            Assert.That(rangeExpressionNode.RightExpression.Inner, Is.InstanceOf<NumberLiteralNode>());
            var rightBandLiteral = (NumberLiteralNode) rangeExpressionNode.RightExpression.Inner;
            Assert.That(rightBandLiteral.Value, Is.EqualTo(9));
        }

        [Test]
        public void ParseExpressionWithLhsNumberLiteral()
        {
            var sut = new DomainExpressionModel("    1..9     ");
            var rangeExpressionNode = (RangeDomainExpressionNode)sut.Node.Inner;
            Assert.That(rangeExpressionNode.LeftExpression.Inner, Is.InstanceOf<NumberLiteralNode>());
            var leftBandLiteral = (NumberLiteralNode) rangeExpressionNode.LeftExpression.Inner;
            Assert.That(leftBandLiteral.Value, Is.EqualTo(1));
        }

        [Test]
        public void ParseExpressionWithSizeFunctionCall()
        {
            var sut = new DomainExpressionModel("1..size(x)");
            var rangeExpressionNode = (RangeDomainExpressionNode)sut.Node.Inner;
            Assert.That(rangeExpressionNode.RightExpression.Inner, Is.InstanceOf(typeof(FunctionInvocationNode)));
        }

        [Test]
        public void ParseExpressionWithListInnerIsListNode()
        {
            var sut = new DomainExpressionModel("\"bob\", \"jim\", \"simon\"");
            Assert.That(sut.Node.Inner, Is.InstanceOf<ListDomainExpressionNode>());
        }

        [Test]
        public void ParseExpressionWithListInnerIsListNode2()
        {
            var sut = new DomainExpressionModel("\"bob\", \"jim\", \"simon\"");
            var listExpressionNode = (ListDomainExpressionNode) sut.Node.Inner;
            var firstListValue = listExpressionNode.Items.Values[0];
            Assert.That(firstListValue.Value, Is.EqualTo("bob"));
        }
    }
}
