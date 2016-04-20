using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class ContraintExpressionParserWithEmptyStatementShould
    {
        [Test]
        public void ParseWithEmptyStatementReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse(string.Empty);
            Assert.That(expressionParseResult.Status, Is.EqualTo(ConstraintExpressionParseStatus.Success));
        }

        [Test]
        public void ParseWithEmptyStatementReturnsEmptyNode()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse(string.Empty);
            var actualX = expressionParseResult.Root;
            Assert.That(actualX.IsEmpty, Is.True);
        }

        private static ConstraintExpressionParser CreateSut()
        {
            return new ConstraintExpressionParser();
        }
    }
}
