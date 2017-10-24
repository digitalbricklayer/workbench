using System.Windows;
using System.Linq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class DomainModelTests
    {
        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Upper_Band()
        {
            var sut = new DomainGraphicModel("A domain", new Point(0, 0), new DomainModel("    1..9     "));
            Assert.That(sut.Expression.Node.LeftExpression, Is.InstanceOf<BandExpressionNode>());
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Lower_Band()
        {
            var sut = new DomainGraphicModel("    1..9     ");
            Assert.That(sut.Expression.Node.RightExpression, Is.InstanceOf<BandExpressionNode>());
        }
    }
}
