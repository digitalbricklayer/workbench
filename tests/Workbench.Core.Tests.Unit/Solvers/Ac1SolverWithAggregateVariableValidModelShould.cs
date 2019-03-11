using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class Ac1SolverWithAggregateVariableValidModelShould
    {
        [Test]
        public void SolveReturningStatusSuccess()
        {
            var sut = new Ac1Solver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveReturningLabelInValidRange()
        {
            var sut = new Ac1Solver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var colsLabel = actualResult.Snapshot.GetCompoundLabelByVariableName("cols");
            Assert.That(colsLabel.Values, Is.All.InRange(1, 4));
        }

        [Test]
        public void SolveReturningLabelSatisfiesConstraint()
        {
            var sut = new Ac1Solver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var colsLabel = actualResult.Snapshot.GetCompoundLabelByVariableName("cols");
            Assert.That(colsLabel.GetValueAt(1), Is.GreaterThan(colsLabel.GetValueAt(3)));
        }

        private WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("Very simple aggregate variable model utilizing a binary constraint")
                            .AddAggregate("cols", 4, "1..size(cols)")
                            .WithConstraintExpression("$cols[1] > $cols[3]")
                            .Build();
        }
    }
}
