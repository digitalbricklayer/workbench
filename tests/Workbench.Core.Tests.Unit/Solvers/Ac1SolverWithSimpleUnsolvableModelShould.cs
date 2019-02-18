using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class Ac1SolverWithSimpleUnsolvableModelShould
    {
        [Test]
        public void SolveReturningStatusFail()
        {
            var sut = new Ac1Solver();
            var actualResult = sut.Solve(CreateModel());
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Fail));
        }

        private ModelModel CreateModel()
        {
            var a = new WorkspaceBuilder("Simple model that cannot be solved")
                            .WithSharedDomain("D", "\"red\", \"blue\"")
                            .AddSingleton("x", "$D")
                            .AddSingleton("y", "$D")
                            .AddSingleton("z", "$D")
                            .WithConstraintExpression("$x = $y")
                            .WithConstraintExpression("$y = $z")
                            .WithConstraintExpression("$x <> $z")
                            .Build();

            return a.Model;
        }
    }
}
