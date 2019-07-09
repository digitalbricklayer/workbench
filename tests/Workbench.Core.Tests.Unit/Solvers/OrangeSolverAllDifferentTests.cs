using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class OrangeSolverAllDifferentTests
    {
        [Test]
        public void SolveWithAllDifferentModelReturnsStatusSuccess()
        {
            var sut = new OrangeSolver();
            var actualResult = sut.Solve(MakeModel());

            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithAllDifferentModelSatisfiesAllDifferentConstraint()
        {
            var sut = new OrangeSolver();
            var actualResult = sut.Solve(MakeModel());

            var actualSnapshot = actualResult.Snapshot;
            var xLabel = actualSnapshot.GetAggregateLabelByVariableName("x");
            Assert.That(xLabel.Values, Is.Unique);
        }

        [Test]
        public void SolveWithAllDifferentModelSolutionHasValidVariableCount()
        {
            var sut = new OrangeSolver();
            var actualResult = sut.Solve(MakeModel());

            var actualSnapshot = actualResult.Snapshot;
            Assert.That(actualSnapshot.SingletonLabels, Is.Empty);
            Assert.That(actualSnapshot.AggregateLabels.Count, Is.EqualTo(1));
        }

        private static ModelModel MakeModel()
        {
            var workspace = new WorkspaceBuilder("A test using an all different constraint on an aggregate variable")
                                          .AddAggregate("x", 3, "1..size(x)")
                                          .WithConstraintAllDifferent("x")
                                          .Build();

            return workspace.Model;
        }
    }
}
