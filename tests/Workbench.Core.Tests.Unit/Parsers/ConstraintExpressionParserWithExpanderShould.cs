using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class ConstraintExpressionParserWithExpanderShould
    {
        [TestCase("$x[i] <> $x[i] + 1 | i in 1..10")]
        [TestCase("$x[i] <> $x[j] | i,j in 1..10,1..10")]
        [TestCase("$x[i] <> $x[j] | i,j in 1..10,1..i")]
        [TestCase("$x[i] <> $x[j] | i,j in 1..10,1..size(x)")]
        [TestCase("$x[i] <> $x[j] | i,j in 10,i")]
        [TestCase("$x[i] + i <> $x[j] + j | i,j in 8,i")]
        [TestCase("$x[i] + i <> $x[j] + j | i,j in size(x),i")]
        public void ParseWithSingleLevelExpanderExpressionReturnsStatusSuccess(string constraintExpression)
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse(constraintExpression);
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        private static ConstraintExpressionParser CreateSut()
        {
            return new ConstraintExpressionParser();
        }
    }
}
