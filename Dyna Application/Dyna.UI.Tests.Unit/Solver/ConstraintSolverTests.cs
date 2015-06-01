using DynaApp.Entities;
using DynaApp.Solver;
using NUnit.Framework;

namespace DynaApp.UI.Tests.Unit.Solver
{
    [TestFixture]
    public class ConstraintSolverTests
    {
        [Test]
        public void Solve_With_Simple_Model_Returns_Status_Success()
        {
            // Arrange
            var sut = new ConstraintSolver();

            // Act
            var actualResult = sut.Solve(MakeSimpleModel());

            // Assert
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void Solve_With_Simple_Model_Satisfies_Constraint()
        {
            // Arrange
            var sut = new ConstraintSolver();

            // Act
            var actualResult = sut.Solve(MakeSimpleModel());

            // Assert
            var actualSolution = actualResult.Solution;
            var x = actualSolution.GetVariableByName("x");
            var y = actualSolution.GetVariableByName("y");
            Assert.That(x.Value, Is.Not.EqualTo(y.Value));
        }

        [Test]
        public void Solve_With_Simple_Model_Solution_Within_Domain()
        {
            // Arrange
            var sut = new ConstraintSolver();

            // Act
            var actualResult = sut.Solve(MakeSimpleModel());

            // Assert
            var actualSolution = actualResult.Solution;
            var x = actualSolution.GetVariableByName("x");
            var y = actualSolution.GetVariableByName("y");
            Assert.That(x.Value, Is.InRange(x.ModelVariable.Domain.Expression.LowerBand, x.ModelVariable.Domain.Expression.UpperBand));
            Assert.That(y.Value, Is.InRange(y.ModelVariable.Domain.Expression.LowerBand, y.ModelVariable.Domain.Expression.UpperBand));
        }

        private static Model MakeSimpleModel()
        {
            return Model.Create("A test")
                        .AddVariable("x")
                        .AddVariable("y")
                        .WithDomain("1..9")
                        .WithConstraint("x != y")
                        .Build();
        }
    }
}
