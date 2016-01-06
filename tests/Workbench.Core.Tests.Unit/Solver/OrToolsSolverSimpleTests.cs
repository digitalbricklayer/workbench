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
            var actualSolution = actualResult.Solution;
            var x = actualSolution.GetSingletonVariableValueByName("x");
            var y = actualSolution.GetSingletonVariableValueByName("y");
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
            var actualSolution = actualResult.Solution;
            var x = actualSolution.GetSingletonVariableValueByName("x");
            var y = actualSolution.GetSingletonVariableValueByName("y");
            Assert.That(x.Value, Is.InRange(1, 9));
            Assert.That(y.Value, Is.InRange(1, 9));
        }

        private static ModelModel MakeModel()
        {
            return ModelModel.Create("A test")
                             .WithSharedDomain("a", "1..9")
                             .AddSingleton("x", "a")
                             .AddSingleton("y", "a")
                             .AddSingleton("z", "a")
                             .WithConstraint("x != y")
                             .WithConstraint("x <= y")
                             .WithConstraint("y = z")
                             .Build();
        }
    }
}
