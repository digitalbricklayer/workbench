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
        public void ParseWithValidAggregateNotEqualToExpressionReturnsStutusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[1] + 1 <> $x[1] + 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithValidSingletonNotEqualToExpressionReturnsStutusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x + 1 <> 1");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithValidCharacterLiteralNotEqualToExpressionReturnsStutusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x <> 'a'");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithValidItemNameNotEqualToExpressionReturnsStutusSuccess()
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
