using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    public class VariableDomainExpressionParserShould
    {
        [TestCase("workers!1:1")]
        [TestCase("1..10")]
        [TestCase("\"bob\", \"jim\", \"sue\"")]
        public void ParseWithValidTableReferenceReturnsStatusSuccess(string variableDomainExpression)
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse(variableDomainExpression);
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        private VariableDomainExpressionParser CreateSut()
        {
            return new VariableDomainExpressionParser();
        }
    }
}
