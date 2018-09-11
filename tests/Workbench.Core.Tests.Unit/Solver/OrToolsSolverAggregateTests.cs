using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class OrToolsSolverAggregateTests
    {
        [Test]
        public void SolveWithModelReturnsStatusSuccess()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());
                Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
            }
        }

        [Test]
        public void SolveWithModelSatisfiesConstraints()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());
                var actualSnapshot = actualResult.Snapshot;
                var c = actualSnapshot.GetCompoundLabelByVariableName("c");
                Assert.That(c.GetValueAt(0), Is.LessThan(c.GetValueAt(9)));
                Assert.That(c.GetValueAt(1), Is.GreaterThan(c.GetValueAt(8)));
            }
        }

        [Test]
        public void SolveWithModelSatisfiesValueCount()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());
                var actualSnapshot = actualResult.Snapshot;
                var c = actualSnapshot.GetCompoundLabelByVariableName("c");
                Assert.That(c.Values.Count, Is.EqualTo(10));
            }
        }

        [Test]
        public void SolveWithModelCreatesValidSolution()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());
                var actualSnapshot = actualResult.Snapshot;
                var c = actualSnapshot.GetCompoundLabelByVariableName("c");
                Assert.That(c.GetValueAt(0), Is.InRange(1, 9));
            }
        }

        [Test]
        public void SolveWithSimpleAggregateModelSolutionHasValidVariableCount()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());

                var actualSnapshot = actualResult.Snapshot;
                var singletonVariableCount = actualSnapshot.SingletonLabels.Count;
                var aggregateVariableCount = actualSnapshot.AggregateLabels.Count;
                Assert.That(singletonVariableCount, Is.EqualTo(0));
                Assert.That(aggregateVariableCount, Is.EqualTo(1));
            }
        }

        private static ModelModel MakeModel()
        {
            var workspace = new WorkspaceBuilder("An aggregate test")
                                          .AddAggregate("c", 10, "1..9")
                                          .WithConstraintExpression("$c[0] < $c[9]")
                                          .WithConstraintExpression("$c[1] > $c[8]")
                                          .Build();

            return workspace.Model;
        }
    }
}
