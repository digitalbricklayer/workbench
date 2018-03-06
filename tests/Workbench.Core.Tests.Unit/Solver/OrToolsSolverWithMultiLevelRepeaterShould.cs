using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class OrToolsSolverWithMultiLevelRepeaterShould
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
                Assert.That(x.GetValueAt(1), Is.EqualTo(x.GetValueAt(2)));
                Assert.That(x.GetValueAt(1), Is.EqualTo(x.GetValueAt(3)));
            }
        }

        private static OrToolsSolver CreateSolver()
        {
            return new OrToolsSolver();
        }

        private static ModelModel MakeModel()
        {
            var workspace = WorkspaceModel.Create("A multi-level repeater test")
                                          .AddAggregate("x", 10, "1..10")
                                          .WithConstraintExpression("$x[i] <> $x[j] + 1 | i,j in 0..9,0..i")
                                          .Build();

            return workspace.Model;
        }
    }
}
