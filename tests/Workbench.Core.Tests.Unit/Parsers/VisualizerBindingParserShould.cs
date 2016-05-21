using System;
using NUnit.Framework;
using Workbench.Core.Parsers;

namespace Workbench.Core.Tests.Unit.Parsers
{
    [TestFixture]
    public class VisualizerBindingParserShould
    {
        [Test]
        public void ParseWithNullStatementThrowsArgumentNullException()
        {
            var sut = CreateSut();
            Assert.Throws<ArgumentNullException>(() => sut.Parse(null));
        }

        private static VisualizerBindingExpressionParser CreateSut()
        {
            return new VisualizerBindingExpressionParser();
        }
    }
}
