using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class OrangeSolverWithExpressionConstraintRepeaterShould
    {
        [Test]
        public void SolveReturningStatusSuccess()
        {
            var sut = new OrangeSolver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveReturningLabelInValidRange()
        {
            var sut = new OrangeSolver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var colsLabel = actualResult.Snapshot.GetAggregateLabelByVariableName("cols");
            Assert.That(colsLabel.Values, Is.All.InRange(2, colsLabel.Values.Count));
        }

        [Test]
        public void SolveReturningLabelSatisfyingConstraint()
        {
            var sut = new OrangeSolver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var colsLabel = actualResult.Snapshot.GetAggregateLabelByVariableName("cols");
            var xLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("x");
            Assert.That(colsLabel.Values, Is.All.GreaterThan(xLabel.Value));
        }

        private WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("Very simple aggregate variable model utilizing a constraint repeater expression constraint")
                            .AddAggregate("cols", 4, "1..size(cols)")
                            .AddSingleton("x", "1..4")
                            .WithConstraintExpression("$cols[i] > $x | i in size(cols)")
                            .Build();
        }
    }
}
