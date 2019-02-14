using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class ConstraintExpressionParserWithEmptyStatementShould
    {
        [Test]
        public void ParseWithEmptyStatementReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse(string.Empty);
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithEmptyStatementReturnsEmptyNode()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse(string.Empty);
            var actualRootNode = expressionParseResult.Root;
            Assert.That(actualRootNode.IsEmpty, Is.True);
        }

        private static ConstraintExpressionParser CreateSut()
        {
            return new ConstraintExpressionParser();
        }
    }
}
