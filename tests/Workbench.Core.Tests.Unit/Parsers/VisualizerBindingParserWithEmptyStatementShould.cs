using System;
using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class VisualizerBindingWithSimpleStatementParserShould
    {
        [Test]
        public void ParseWithEmptyStatementReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var expressionParseResult = sut.Parse(string.Empty);
            Assert.That(expressionParseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        private static VisualizerBindingExpressionParser CreateSut()
        {
            return new VisualizerBindingExpressionParser();
        }
    }
}
