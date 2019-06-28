using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class OrangeSolverWithTernaryAndBinaryConstraintValidModelShould
    {
        [Test]
        public void SolveReturningStatusSuccess()
        {
            var sut = new OrangeSolver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveReturningLabelXInValidRange()
        {
            var sut = new OrangeSolver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var xLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("x");
            Assert.That(xLabel.GetValueAsInt(), Is.InRange(1, 2));
        }

        [Test]
        public void SolveReturningLabelYInValidRange()
        {
            var sut = new OrangeSolver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var yLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("y");
            Assert.That(yLabel.GetValueAsInt(), Is.InRange(1, 2));
        }

        [Test]
        public void SolveReturningLabelsSatisfyConstraint()
        {
            var sut = new OrangeSolver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var xLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("x");
            var yLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("y");
            Assert.That(xLabel.GetValueAsInt(), Is.GreaterThan(yLabel.GetValueAsInt()));
        }

        private WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("Very simple model utilizing both a ternary and binary constraint")
                            .AddSingleton("x", "1..2")
                            .AddSingleton("y", "1..2")
                            .WithConstraintExpression("$x <> $y")
                            .WithConstraintExpression("$x > $y")
                            .Build();
        }
    }
}
