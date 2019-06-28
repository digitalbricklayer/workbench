using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class OrangeSolverWithConstraintUsingEqualsModelShould
    {
        [Test]
        public void SolveReturningStatusSuccess()
        {
            var sut = new OrangeSolver();
            var actualResult = sut.Solve(CreateWorkspace().Model);
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveReturningLabelInValidRange()
        {
            var sut = new OrangeSolver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var xLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("x");
            var yLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("y");
            Assert.That(xLabel.Value, Is.AnyOf("red", "blue"));
            Assert.That(yLabel.Value, Is.AnyOf("red", "blue"));
        }

        [Test]
        public void SolveReturningLabelSatisfyingConstraint()
        {
            var sut = new OrangeSolver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var xLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("x");
            var yLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("y");
            Assert.That(xLabel.Value, Is.EqualTo(yLabel.Value));
        }

        private WorkspaceModel CreateWorkspace()
        {
            var a = new WorkspaceBuilder("Simple model with ternary operator that can be solved")
                            .WithSharedDomain("D", "\"red\", \"blue\"")
                            .AddSingleton("x", "$D")
                            .AddSingleton("y", "$D")
                            .WithConstraintExpression("$x = $y")
                            .Build();

            return a;
        }
    }
}
