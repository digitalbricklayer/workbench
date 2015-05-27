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
            var theModel = MakeTheModel();

            // Act
            var actualResult = sut.Solve(theModel);

            // Assert
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        private static Model MakeTheModel()
        {
            var theModel = new Model("A test");
            theModel.AddDomain(Entities.Domain.CreateFrom(1, 2, 3, 4, 5, 6, 7, 8, 9));
            var x = new Variable("x");
            theModel.AddVariable(x);
            var y = new Variable("y");
            theModel.AddVariable(y);
            theModel.AddConstraint(new Constraint("x > y"));
            return theModel;
        }
    }
}
