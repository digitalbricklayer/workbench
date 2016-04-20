using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class ContraintExpressionParserShould
    {
        [Test]
        public void ParseWithValidGreaterThanExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("x > 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ConstraintExpressionParseStatus.Success));
        }

        [Test]
        public void ParseWithValidNotEqualExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("x[1] != 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ConstraintExpressionParseStatus.Success));
        }

        [Test]
        public void ParseWithValidBackwardsNotEqualExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("1 <> x[1]");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ConstraintExpressionParseStatus.Success));
        }

        [Test]
        public void ParseWithValidAggregateNotEqualToExpressionReturnsStutusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("x[1] + 1 <> x[1] + 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ConstraintExpressionParseStatus.Success));
        }

        [Test]
        public void ParseWithValidSingletonNotEqualToExpressionReturnsStutusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("x + 1 <> 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ConstraintExpressionParseStatus.Success));
        }

        private static ConstraintExpressionParser CreateSut()
        {
            return new ConstraintExpressionParser();
        }
    }
}
