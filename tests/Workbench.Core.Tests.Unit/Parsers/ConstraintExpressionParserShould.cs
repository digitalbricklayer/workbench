using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class ConstraintExpressionParserShould
    {
        [Test]
        public void ParseWithValidGreaterThanExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x > 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithValidNotEqualExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[1] != 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithValidBackwardsNotEqualExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("1 <> $x[1]");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithValidAggregateNotEqualToExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[1] + 1 <> $x[1] + 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithValidSingletonNotEqualToExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x + 1 <> 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithValidCharacterLiteralNotEqualToExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x <> 'a'");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithValidItemNameNotEqualToExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x <> john");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        private ConstraintExpressionParser CreateSut()
        {
            return new ConstraintExpressionParser();
        }
    }
}
