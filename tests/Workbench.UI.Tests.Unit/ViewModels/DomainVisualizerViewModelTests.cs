using NUnit.Framework;

namespace Workbench.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class DomainVisualizerViewModelTests
    {
        [Test]
        public void IsValid_With_Valid_Expression_Returns_True()
        {
            var sut = new DomainBuilder().WithName("X")
                                         .WithDomain("1..2")
                                         .Build();
            Assert.That(sut.DomainEditor.IsValid, Is.True);
        }

        [Test]
        public void IsValid_With_Empty_Expression_Returns_False()
        {
            var sut = new DomainBuilder().WithName("X")
                                         .Build();
            Assert.That(sut.DomainEditor.IsValid, Is.False);
        }

        [Test]
        public void UpdateDomainExpressionUpdatesModel()
        {
            var sut = new DomainBuilder().Build();
            sut.DomainEditor.Expression.Text = "1..10";
            Assert.That(sut.DomainEditor.Expression.Model.Node, Is.Not.Null);
        }
    }
}
