using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit.Models
{
    [TestFixture]
    public class DisplayModelShould
    {
        [Test]
        public void AddVisualizerExpressionAssignsId()
        {
            var sut = CreateDisplayModel();
            var newVisualizerExpression = new VisualizerBindingExpressionModel("states(row:7,column:2,Text:<t>)");
            sut.AddBindingExpression(newVisualizerExpression);
            Assert.That(newVisualizerExpression.HasIdentity, Is.True);
        }

        private DisplayModel CreateDisplayModel()
        {
            return new DisplayModel(WorkspaceModelFactory.Create().Model);
        }
    }
}
