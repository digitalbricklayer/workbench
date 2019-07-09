using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class OrangeSolverBugReproduction
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
            var xLabel = actualSnapshot.GetSingletonLabelByVariableName("x");
            var yLabel = actualSnapshot.GetSingletonLabelByVariableName("y");
            var zLabel = actualSnapshot.GetSingletonLabelByVariableName("z");
            Assert.That(xLabel.Value, Is.Not.EqualTo(yLabel.Value));
            Assert.That(xLabel.Value, Is.Not.EqualTo(zLabel.Value));
            Assert.That(yLabel.Value, Is.Not.EqualTo(zLabel.Value));
        }

        [Test]
        public void SolveWithAllDifferentModelSolutionHasValidVariableCount()
        {
            var sut = new OrangeSolver();
            var actualResult = sut.Solve(MakeModel());

            var actualSnapshot = actualResult.Snapshot;
            Assert.That(actualSnapshot.SingletonLabels.Count, Is.EqualTo(3));
            Assert.That(actualSnapshot.AggregateLabels, Is.Empty);
        }

        private static ModelModel MakeModel()
        {
            var workspace = new WorkspaceBuilder("A test to reproduce a solver bug")
                                          .AddSingleton("x", "1..3")
                                          .AddSingleton("y", "1..3")
                                          .AddSingleton("z", "1..3")
										  .WithConstraintExpression("$x <> $y")
										  .WithConstraintExpression("$x <> $z")
										  .WithConstraintExpression("$y <> $z")
                                          .Build();

            return workspace.Model;
        }
    }
}
