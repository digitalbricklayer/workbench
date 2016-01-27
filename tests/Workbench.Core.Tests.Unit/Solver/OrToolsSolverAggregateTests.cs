using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class OrToolsSolverAggregateTests
    {
        [Test]
        public void SolveWithModelReturnsStatusSuccess()
        {
            var sut = new OrToolsSolver();
            var actualResult = sut.Solve(MakeModel());
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithModelSatisfiesConstraints()
        {
            var sut = new OrToolsSolver();
            var actualResult = sut.Solve(MakeModel());
            var actualSolution = actualResult.Solution;
            var c = actualSolution.GetAggregateVariableValueByName("c");
            Assert.That(c.GetValueAt(1), Is.LessThan(c.GetValueAt(10)));
            Assert.That(c.GetValueAt(2), Is.GreaterThan(c.GetValueAt(9)));
        }

        [Test]
        public void SolveWithModelSatisfiesValueCount()
        {
            var sut = new OrToolsSolver();
            var actualResult = sut.Solve(MakeModel());
            var actualSolution = actualResult.Solution;
            var c = actualSolution.GetAggregateVariableValueByName("c");
            Assert.That(c.Values, Has.Count.EqualTo(10));
        }

        [Test]
        public void SolveWithModelCreatesValidSolution()
        {
            var sut = new OrToolsSolver();
            var actualResult = sut.Solve(MakeModel());
            var actualSolution = actualResult.Solution;
            var c = actualSolution.GetAggregateVariableValueByName("c");
            Assert.That(c.GetValueAt(1), Is.InRange(1, 9));
        }

        private static ModelModel MakeModel()
        {
            var workspace = WorkspaceModel.Create("An aggregate test")
                                          .AddAggregate("c", 10, "1..9")
                                          .WithConstraint("c[1] < c[10]")
                                          .WithConstraint("c[2] > c[9]")
                                          .Build();

            return workspace.Model;
        }
    }
}
