using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class OrToolsSolverAllDifferentTests
    {
        [Test]
        public void SolveWithAllDifferentModelReturnsStatusSuccess()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());

                Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
            }
        }

        [Test]
        public void SolveWithAllDifferentModelSatisfiesConstraints()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());

                var actualSnapshot = actualResult.Snapshot;
                var x = actualSnapshot.GetCompoundLabelByVariableName("x");
                Assert.That(x.Values, Is.Unique);
            }
        }

        [Test]
        public void SolveWithAllDifferentModelSolutionHasValidVariableCount()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());

                var actualSnapshot = actualResult.Snapshot;
                var aggregateVariableCount = actualSnapshot.AggregateLabels.Count;
                Assert.That(actualSnapshot.SingletonLabels, Is.Empty);
                Assert.That(aggregateVariableCount, Is.EqualTo(1));
            }
        }

        private static ModelModel MakeModel()
        {
            var workspace = new WorkspaceBuilder("A test")
                                          .WithSharedDomain("a", "1..9")
                                          .AddAggregate("x", 10, "1..10")
                                          .WithConstraintAllDifferent("x")
                                          .Build();

            return workspace.Model;
        }
    }
}
