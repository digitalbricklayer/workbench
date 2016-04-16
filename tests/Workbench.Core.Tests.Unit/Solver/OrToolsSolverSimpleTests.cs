using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class OrToolsSolverSimpleTests
    {
        [Test]
        public void Solve_With_Simple_Model_Returns_Status_Success()
        {
            // Arrange
            var sut = new OrToolsSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void Solve_With_Simple_Model_Satisfies_Constraint()
        {
            // Arrange
            var sut = new OrToolsSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            var actualSnapshot = actualResult.Snapshot;
            var x = actualSnapshot.GetSingletonVariableValueByName("x");
            var y = actualSnapshot.GetSingletonVariableValueByName("y");
            Assert.That(x.Value, Is.Not.EqualTo(y.Value));
        }

        [Test]
        public void Solve_With_Simple_Model_Solution_Within_Domain()
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
            var aggregateVariableCount = actualSnapshot.AggregateValues.Count;
            Assert.That(singletonVariableCount, Is.EqualTo(3));
            Assert.That(aggregateVariableCount, Is.EqualTo(0));
        }

        private static ModelModel MakeModel()
        {
            var workspace = WorkspaceModel.Create("A test")
                                          .WithSharedDomain("a", "1..9")
                                          .AddSingleton("x", "a")
                                          .AddSingleton("y", "a")
                                          .AddSingleton("z", "a")
                                          .WithConstraintExpression("x != y")
                                          .WithConstraintExpression("x <= y")
                                          .WithConstraintExpression("y = z")
                                          .Build();

            return workspace.Model;
        }
    }
}
