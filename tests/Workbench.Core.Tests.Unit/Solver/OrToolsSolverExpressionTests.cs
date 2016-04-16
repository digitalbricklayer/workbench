using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class OrToolsSolverExpressionTests
    {
        [Test]
        public void SolveWithExpressionModelReturnsStatusSuccess()
        {
            // Arrange
            var sut = new OrToolsSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithExpressionModelSatisfiesConstraints()
        {
            // Arrange
            var sut = new OrToolsSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            var actualSnapshot = actualResult.Snapshot;
            var x = actualSnapshot.GetSingletonVariableValueByName("x");
            var y = actualSnapshot.GetSingletonVariableValueByName("y");
            Assert.That(x.Value + 1, Is.Not.EqualTo(y.Value - 1));
        }

        [Test]
        public void SolveWithExpressionModelSolutionWithinDomain()
        {
            // Arrange
            var sut = new OrToolsSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            var actualSnapshot = actualResult.Snapshot;
            var x = actualSnapshot.GetSingletonVariableValueByName("x");
            var y = actualSnapshot.GetSingletonVariableValueByName("y");
            Assert.That(x.Value, Is.InRange(1, 9));
            Assert.That(y.Value, Is.InRange(1, 9));
        }

        [Test]
        public void SolveWithSimpleModelSolutionHasValidVariableCount()
        {
            // Arrange
            var sut = new OrToolsSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            var actualSnapshot = actualResult.Snapshot;
            var singletonVariableCount = actualSnapshot.SingletonValues.Count;
            Assert.That(singletonVariableCount, Is.EqualTo(3));
            Assert.That(actualSnapshot.AggregateValues, Is.Empty);
        }

        private static ModelModel MakeModel()
        {
            var workspace = WorkspaceModel.Create("A test")
                                          .WithSharedDomain("a", "1..9")
                                          .AddSingleton("x", "a")
                                          .AddSingleton("y", "a")
                                          .AddSingleton("z", "a")
                                          .WithConstraintExpression("x + 1 != y - 1")
                                          .WithConstraintExpression("x <= y")
                                          .WithConstraintExpression("y = z")
                                          .Build();

            return workspace.Model;
        }
    }
}
