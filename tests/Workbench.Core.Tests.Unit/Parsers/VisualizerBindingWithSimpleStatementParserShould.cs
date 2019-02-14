using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class VisualizerBindingWithSimpleStatementParserShould
    {
        [TestCase("board(x:7,y:7,side:white,piece:queen)")]
        [TestCase("if <cols,1> = 1: board(x:7,y:7,side:white,piece:queen)")]
        [TestCase("if <cols> = 1: board(x:7,y:7,side:white,piece:queen)")]
        [TestCase("for i in 1..8: board(x:7,y:7,side:white,piece:queen)")]
        [TestCase("for i in 1..8: if <x,1> = 1: board(x:7,y:7,side:white,piece:queen)")]
        [TestCase("for i,j in 1..8,1..8: if <cols,%i> = %j: board(x:7,y:7,side:white,piece:queen)")]
        [TestCase("for i,j in 1..size(cols),1..size(cols): if <cols,%i> = %j: board(x:7,y:7,side:white,piece:queen)")]
        [TestCase("if <cols,1> = jim: board(x:7,y:7,side:white,piece:queen)")]
        public void ParseWithValidVisualizerStatementReturnsStatusSuccess(string visualizerBindingExpression)
        {
            var sut = CreateSut();
            var parseResult = sut.Parse(visualizerBindingExpression);
            Assert.That(parseResult.Status, Is.EqualTo(ParseStatus.Success));
        }

        private static VisualizerBindingExpressionParser CreateSut()
        {
            return new VisualizerBindingExpressionParser();
        }
    }
}
