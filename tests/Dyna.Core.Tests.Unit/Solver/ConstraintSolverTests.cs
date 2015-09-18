using Dyna.Core.Models;
using Dyna.Core.Solver;
using NUnit.Framework;

namespace Dyna.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class ConstraintSolverTests
    {
        [Test]
        public void Solve_With_Model_Returns_Status_Success()
        {
            // Arrange
            var sut = new ConstraintSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void Solve_With_Model_Satisfies_Constraint()
        {
            // Arrange
            var sut = new ConstraintSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            var actualSolution = actualResult.Solution;
            var x = actualSolution.GetVariableByName("x");
            var y = actualSolution.GetVariableByName("y");
            Assert.That(x.Value, Is.Not.EqualTo(y.Value));
        }

        [Test]
        public void Solve_With_Model_Solution_Within_Domain()
        {
            // Arrange
            var sut = new ConstraintSolver();

            // Act
            var actualResult = sut.Solve(MakeModel());

            // Assert
            var actualSolution = actualResult.Solution;
            var x = actualSolution.GetVariableByName("x");
            var y = actualSolution.GetVariableByName("y");
            Assert.That(x.Value, Is.InRange(x.Variable.Domain.Expression.LowerBand, x.Variable.Domain.Expression.UpperBand));
            Assert.That(y.Value, Is.InRange(y.Variable.Domain.Expression.LowerBand, y.Variable.Domain.Expression.UpperBand));
        }

        private static ModelModel MakeModel()
        {
            return ModelModel.Create("A test")
                        .AddVariable("x", new DomainModel("1..9"))
                        .AddVariable("y", new DomainModel("1..9"))
                        .AddVariable("z", new DomainModel("1..9"))
                        .WithConstraint("x < y")
                        .WithConstraint("y > z")
                        .Build();
        }
    }
}
