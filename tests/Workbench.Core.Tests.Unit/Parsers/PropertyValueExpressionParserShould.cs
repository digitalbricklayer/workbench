using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    public class PropertyValueExpressionParserShould
    {
        [TestCase("")]
        [TestCase("<x>")]
        [TestCase("<x,1>")]
        public void ParseWithValidPropertyUpdateExpressionReturnsStatusSuccess(string propertyUpdateExpression)
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse(propertyUpdateExpression);
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        private PropertyBindingExpressionParser CreateSut()
        {
            return new PropertyBindingExpressionParser();
        }
    }
}
