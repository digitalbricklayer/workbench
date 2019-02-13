using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class ConstraintExpressionParserWithExpanderShould
    {
        [Test]
        public void ParseWithSingleLevelExpanderExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[i] <> $x[i] + 1 | i in 1..10");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithMultiLevelExpanderExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[i] <> $x[j] | i,j in 1..10,1..10");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithMultiLevelExpanderUsingVariableScopeExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[i] <> $x[j] | i,j in 1..10,1..i");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithMultiLevelExpanderUsingSizeFunctionExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[i] <> $x[j] | i,j in 1..10,1..size(x)");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithMultiLevelExpanderUsingVariableLimitExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[i] <> $x[j] | i,j in 10,i");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithMultiLevelExpanderUsingInfixStatementReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[i] + i <> $x[j] + j | i,j in 8,i");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithMultiLevelExpanderUsingSizeFunctionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse("$x[i] + i <> $x[j] + j | i,j in size(x),i");
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseEmptyExpressionReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse(string.Empty);
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        private static ConstraintExpressionParser CreateSut()
        {
            return new ConstraintExpressionParser();
        }
    }
}
