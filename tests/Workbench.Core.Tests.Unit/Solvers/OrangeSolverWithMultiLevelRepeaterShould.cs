using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class OrangeSolverWithMultiLevelRepeaterShould
    {
        [Test]
        public void SolveWithRepeaterConstraintReturnsStatusSuccess()
        {
            var sut = CreateSolver();
            var actualResult = sut.Solve(MakeModel());
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithRepeaterSatisfiesConstraints()
        {
            var sut = CreateSolver();
            var actualResult = sut.Solve(MakeModel());
            var actualSnapshot = actualResult.Snapshot;
            var xLabel = actualSnapshot.GetAggregateLabelByVariableName("x");
            Assert.That(xLabel.GetValueAt(1), Is.Not.EqualTo(xLabel.GetValueAt(2)));
            Assert.That(xLabel.GetValueAt(1), Is.Not.EqualTo(xLabel.GetValueAt(3)));
        }

        [Test]
        public void SolveReturningLabelInValidRange()
        {
            var sut = CreateSolver();
            var actualResult = sut.Solve(MakeModel());
            var actualSnapshot = actualResult.Snapshot;
            var xLabel = actualSnapshot.GetAggregateLabelByVariableName("x");
            Assert.That(xLabel.Values, Is.All.InRange(1, 10));
        }

        private static OrangeSolver CreateSolver()
        {
            return new OrangeSolver();
        }

        private static ModelModel MakeModel()
        {
            var workspace = new WorkspaceBuilder("A multi-level repeater test")
                                          .AddAggregate("x", 8, "1..size(x)")
                                          .WithConstraintExpression("$x[i] <> $x[j] | i,j in size(x),i")
                                          .Build();

            return workspace.Model;
        }
    }
}
