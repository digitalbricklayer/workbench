using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class OrToolsSolverWithSingleLevelRepeaterShould
    {
        [Test]
        public void SolveWithRepeaterConstraintReturnsStatusSuccess()
        {
            using (var sut = CreateSolver())
            {
                var actualResult = sut.Solve(MakeModel());
                Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
            }
        }

        [Test]
        public void SolveWithRepeaterSatisfiesConstraints()
        {
            using (var sut = CreateSolver())
            {
                var actualResult = sut.Solve(MakeModel());
                var actualSnapshot = actualResult.Snapshot;
                var x = actualSnapshot.GetCompoundLabelByVariableName("x");
                Assert.That(x.GetValueAt(0), Is.Not.EqualTo(x.GetValueAt(1)));
                Assert.That(x.GetValueAt(0), Is.Not.EqualTo(x.GetValueAt(2)));
            }
        }

        private static OrToolsSolver CreateSolver()
        {
            return new OrToolsSolver();
        }

        private static ModelModel MakeModel()
        {
            var workspace = WorkspaceModel.Create("A simple repeater test")
                                          .AddAggregate("x", 10, "1..10")
                                          .WithConstraintExpression("$x[0] <> $x[i] | i in 1..9")
                                          .Build();

            return workspace.Model;
        }
    }
}
