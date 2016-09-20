using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class VisualizerBindingWithSimpleStatementParserShould
    {
        [Test]
        public void ParseWithVisualizerCallStatementReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var parseResult = sut.Parse("board(x:7,y:7,side:white,piece:queen)");
            Assert.That(parseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithIfStatementAggregateReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var parseResult = sut.Parse("if cols[1] = 1: board(x:7,y:7,side:white,piece:queen)");
            Assert.That(parseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        public void ParseWithIfStatementSingletonReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var parseResult = sut.Parse("if cols = 1: board(x:7,y:7,side:white,piece:queen)");
            Assert.That(parseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        [Ignore("")]
        public void ParseWithSingleRepeaterStatementReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var parseResult = sut.Parse("i in 1..8: board(x:7,y:7,side:white,piece:queen)");
            Assert.That(parseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        [Ignore("")]
        public void ParseWithSingleRepeaterIfStatementReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var parseResult = sut.Parse("i in 1..8: if x[1] = 1: board(x:7,y:7,side:white,piece:queen)");
            Assert.That(parseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        [Test]
        [Ignore("")]
        public void ParseWithMultiRepeaterStatementReturnsStatusSuccess()
        {
            var sut = CreateSut();
            var parseResult = sut.Parse("i,j in 1..8,1..8: if cols[i] = j: board(x:7,y:7,side:white,piece:queen)");
            Assert.That(parseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        private static VisualizerBindingExpressionParser CreateSut()
        {
            return new VisualizerBindingExpressionParser();
        }
    }
}
