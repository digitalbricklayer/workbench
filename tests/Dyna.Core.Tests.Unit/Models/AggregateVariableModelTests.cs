using Dyna.Core.Models;
using NUnit.Framework;

namespace Dyna.Core.Tests.Unit.Models
{
    [TestFixture]
    public class AggregateVariableModelTests
    {
        [Test]
        public void InitializeAggregateWithValidNameSetsExpectedNameInVariable()
        {
            var sut = new AggregateVariableModel("x");
            Assert.That(sut.Name, Is.EqualTo("x"));
        }

        [Test]
        public void ResizeAggregateWithZeroSetsNewSize()
        {
            var sut = new AggregateVariableModel("x");
            sut.Resize(0);
            Assert.That(sut.AggregateCount, Is.EqualTo(0));
        }

        [Test]
        public void ResizeAggregateWithTenSetsNewSize()
        {
            var sut = new AggregateVariableModel("x");
            sut.Resize(10);
            Assert.That(sut.AggregateCount, Is.EqualTo(10));
        }

        [Test]
        public void ResizeAggregateToGreaterSIzeAllVariablesNotNull()
        {
            var sut = new AggregateVariableModel("x");
            sut.Resize(10);
            Assert.That(sut.Variables, Is.All.InstanceOf<VariableModel>());
        }

        [Test]
        public void ResizeAggregateToSmallerSizeAllVariablesNotNull()
        {
            var sut = new AggregateVariableModel("x");
            sut.Resize(10);
            sut.Resize(5);
            Assert.That(sut.Variables, Is.All.InstanceOf<VariableModel>());
        }
    }
}
