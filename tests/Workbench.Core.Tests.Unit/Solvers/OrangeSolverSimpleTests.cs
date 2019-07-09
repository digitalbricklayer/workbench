using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class OrangeSolverSimpleTests
    {
        [Test]
        public void Solve_With_Simple_Model_Returns_Status_Success()
        {
            var sut = new OrangeSolver();
            var actualResult = sut.Solve(MakeModel());

            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void Solve_With_Simple_Model_Satisfies_Constraint()
        {
            var sut = new OrangeSolver();
            var actualResult = sut.Solve(MakeModel());

            var actualSnapshot = actualResult.Snapshot;
            var x = actualSnapshot.GetSingletonLabelByVariableName("x");
            var y = actualSnapshot.GetSingletonLabelByVariableName("y");
            Assert.That(x.Value, Is.Not.EqualTo(y.Value));
        }

        [Test]
        public void Solve_With_Simple_Model_Solution_Within_Domain()
        {
            var sut = new OrangeSolver();
            var actualResult = sut.Solve(MakeModel());

            var actualSnapshot = actualResult.Snapshot;
            var x = actualSnapshot.GetSingletonLabelByVariableName("x");
            var y = actualSnapshot.GetSingletonLabelByVariableName("y");
            Assert.That(x.Value, Is.InRange(1, 9));
            Assert.That(y.Value, Is.InRange(1, 9));
        }

        [Test]
        public void SolveWithSimpleModelSolutionHasValidVariableCount()
        {
            var sut = new OrangeSolver();
            var actualResult = sut.Solve(MakeModel());

            var actualSnapshot = actualResult.Snapshot;
            var singletonVariableCount = actualSnapshot.SingletonLabels.Count;
            var aggregateVariableCount = actualSnapshot.AggregateLabels.Count;
            Assert.That(singletonVariableCount, Is.EqualTo(3));
            Assert.That(aggregateVariableCount, Is.EqualTo(0));
        }

        private static ModelModel MakeModel()
        {
            var workspace = new WorkspaceBuilder("A test")
                                          .WithSharedDomain("a", "1..9")
                                          .AddSingleton("x", "$a")
                                          .AddSingleton("y", "$a")
                                          .AddSingleton("z", "$a")
                                          .WithConstraintExpression("$x != $y")
                                          .WithConstraintExpression("$x <= $y")
                                          .WithConstraintExpression("$y = $z")
                                          .Build();

            return workspace.Model;
        }
    }
}
