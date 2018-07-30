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
            var sut = new DomainModel(new ModelName("A domain"), new DomainExpressionModel("    1..9     "));
            var rangeExpressionNode = (RangeDomainExpressionNode) sut.Expression.Node.Inner;
            Assert.That(rangeExpressionNode.LeftExpression, Is.InstanceOf<BandExpressionNode>());
        }

        [Test]
        public void Initialize_With_Raw_Expression_Parses_Expected_Lower_Band()
        {
            var sut = new DomainModel(new DomainExpressionModel("    1..9     "));
            var rangeExpressionNode = (RangeDomainExpressionNode) sut.Expression.Node.Inner;
            Assert.That(rangeExpressionNode.RightExpression, Is.InstanceOf<BandExpressionNode>());
        }

        [Test]
        public void Initialize_With_Character_Range_Parses_Expected_Lower_Band()
        {
            var sut = new DomainModel(new DomainExpressionModel("    'a'..'z'     "));
            var rangeExpressionNode = (RangeDomainExpressionNode)sut.Expression.Node.Inner;
            Assert.That(rangeExpressionNode.RightExpression, Is.InstanceOf<BandExpressionNode>());
        }

        [Test]
        public void Initialize_With_List_Expression_Parses_Expected_Inner()
        {
            var sut = new DomainModel(new DomainExpressionModel("rita, sue, bob"));
            var rangeExpressionNode = (ListDomainExpressionNode)sut.Expression.Node.Inner;
            Assert.That(rangeExpressionNode.Items.Values, Is.All.Not.Null);
            Assert.That(rangeExpressionNode.Items.Values, Is.All.InstanceOf<ItemNameNode>());
            Assert.That(rangeExpressionNode.Items.Values, Has.Count.EqualTo(3));
        }
    }
}
