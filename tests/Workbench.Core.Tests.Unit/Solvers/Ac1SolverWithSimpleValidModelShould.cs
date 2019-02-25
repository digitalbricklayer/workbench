using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class Ac1SolverWithSimpleValidModelShould
    {
        [Test]
        public void SolveReturningStatusSuccess()
        {
            var sut = new Ac1Solver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        private WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("Very simple model utilizing a binary constraint")
                            .AddSingleton("A", "1..2")
                            .AddSingleton("B", "1..2")
                            .WithConstraintExpression("$A > $B")
                            .Build();
        }
    }
}
