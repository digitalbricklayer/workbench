using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class OrToolsSolverAllDifferentTests
    {
        [Test]
        public void SolveWithAllDifferentModelReturnsStatusSuccess()
        {
            // Arrange
            var sut = new OrToolsSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithAllDifferentModelSatisfiesConstraints()
        {
            // Arrange
            var sut = new OrToolsSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            var actualSnapshot = actualResult.Snapshot;
            var x = actualSnapshot.GetAggregateVariableValueByName("x");
            Assert.That(x.Values, Is.Unique);
        }

        [Test]
        public void SolveWithAllDifferentModelSolutionHasValidVariableCount()
        {
            // Arrange
            var sut = new OrToolsSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            var actualSnapshot = actualResult.Snapshot;
            var aggregateVariableCount = actualSnapshot.AggregateValues.Count;
            Assert.That(actualSnapshot.SingletonValues, Is.Empty);
            Assert.That(aggregateVariableCount, Is.EqualTo(1));
        }

        private static ModelModel MakeModel()
        {
            var workspace = WorkspaceModel.Create("A test")
                                          .WithSharedDomain("a", "1..9")
                                          .AddAggregate("x", 10, "1..10")
                                          .WithConstraintAllDifferent("x")
                                          .Build();

            return workspace.Model;
        }
    }
}
