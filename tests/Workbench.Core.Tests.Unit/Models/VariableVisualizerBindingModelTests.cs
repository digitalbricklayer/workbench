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
        public void HasBindingWithUnboudBindingReturnsFalse()
        {
            var sut = new VariableVisualizerBindingModel(this.visualizer);
            Assert.That(sut.HasBinding, Is.False);
        }

        [Test]
        public void HasBindingWithBoudBindingReturnsTrue()
        {
            var sut = new VariableVisualizerBindingModel(this.visualizer, new VariableModel("x"));
            Assert.That(sut.HasBinding, Is.True);
        }
    }
}
