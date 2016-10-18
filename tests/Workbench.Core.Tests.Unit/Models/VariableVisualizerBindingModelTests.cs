using System.Windows;
using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class VariableVisualizerBindingModelTests
    {
        private VariableVisualizerModel visualizer;

        [SetUp]
        public void Initialize()
        {
            this.visualizer = new VariableVisualizerModel(new Point());
        }

        [Test]
        public void HasBindingWithUnboundBindingReturnsFalse()
        {
            var sut = new VariableVisualizerBindingModel(this.visualizer);
            Assert.That(sut.HasBinding, Is.False);
        }

        [Test]
        public void HasBindingWithNewlyBoundBindingReturnsTrue()
        {
            var sut = new VariableVisualizerBindingModel(this.visualizer);
            sut.BindTo(new VariableModel("x"));
            Assert.That(sut.HasBinding, Is.True);
        }

        [Test]
        public void HasBindingWithBoundBindingReturnsTrue()
        {
            var sut = new VariableVisualizerBindingModel(this.visualizer, new VariableModel("x"));
            Assert.That(sut.HasBinding, Is.True);
        }
    }
}
