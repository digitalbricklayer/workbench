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

        private static Model MakeSimpleModel()
        {
            return Model.Create("A test")
                        .AddVariable("x")
                        .AddVariable("y")
                        .WithDomain(Entities.Domain.CreateFrom(1, 2, 3, 4, 5, 6, 7, 8, 9))
                        .WithConstraint("x > y")
                        .Build();
        }
    }
}
