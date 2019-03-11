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

        [Test]
        public void SolveReturningLabelAInValidRange()
        {
            var sut = new Ac1Solver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var aLabel = actualResult.Snapshot.GetLabelByVariableName("A");
            Assert.That(aLabel.GetValueAsInt(), Is.InRange(1, 4));
        }

        [Test]
        public void SolveReturningLabelBInValidRange()
        {
            var sut = new Ac1Solver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var bLabel = actualResult.Snapshot.GetLabelByVariableName("B");
            Assert.That(bLabel.GetValueAsInt(), Is.InRange(1, 4));
        }

        [Test]
        public void SolveReturningLabelCInValidRange()
        {
            var sut = new Ac1Solver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var cLabel = actualResult.Snapshot.GetLabelByVariableName("C");
            Assert.That(cLabel.GetValueAsInt(), Is.InRange(1, 4));
        }

        [Test]
        public void SolveReturningLabelsSatisfyConstraint()
        {
            var sut = new Ac1Solver();
            var simpleModel = CreateWorkspace().Model;
            var actualResult = sut.Solve(simpleModel);
            var aLabel = actualResult.Snapshot.GetLabelByVariableName("A");
            var bLabel = actualResult.Snapshot.GetLabelByVariableName("B");
            Assert.That(aLabel.GetValueAsInt(), Is.GreaterThan(bLabel.GetValueAsInt()));
        }

        private WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("Very simple model utilizing a binary constraint")
                            .AddSingleton("A", "1..4")
                            .AddSingleton("B", "1..4")
                            .AddSingleton("C", "1..4")
                            .WithConstraintExpression("$A > $B")
                            .Build();
        }
    }
}
