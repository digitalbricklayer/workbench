using System.Collections;
using System.Windows;
using NUnit.Framework;

namespace Workbench.Core.Tests.Unit
{
    [TestFixture]
    public class BoardConverterTests
    {
        [Test, TestCaseSource(typeof(BoardConverterCaseSource), nameof(BoardConverterCaseSource.TestCases))]
        public Point ToLocationReturnsCorrectLocation(int counter)
        {
            return BoardConvert.ToPoint(counter);
        }

        public class BoardConverterCaseSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(1).Returns(new Point(1, 1));
                    yield return new TestCaseData(9).Returns(new Point(2, 1));
                    yield return new TestCaseData(16).Returns(new Point(2, 8));
                    yield return new TestCaseData(64).Returns(new Point(8, 8));
                }
            }
        }
    }
}
