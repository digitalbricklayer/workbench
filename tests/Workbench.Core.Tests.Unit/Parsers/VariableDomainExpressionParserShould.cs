using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    public class VariableDomainExpressionParserShould
    {
        [TestCase("")]
        [TestCase("workers!Names:Names")]
        [TestCase("workers!Names1:Names10")]
        [TestCase("workers!Names1,Names2,Names3")]
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
