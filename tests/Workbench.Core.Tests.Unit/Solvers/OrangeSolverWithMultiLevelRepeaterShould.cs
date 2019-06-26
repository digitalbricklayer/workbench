﻿using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    [TestFixture]
    public class OrangeSolverWithMultiLevelRepeaterShould
    {
        [Test]
        public void SolveWithRepeaterConstraintReturnsStatusSuccess()
        {
            using (var sut = CreateSolver())
            {
                var actualResult = sut.Solve(MakeModel());
                Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
            }
        }

        [Test]
        public void SolveWithRepeaterSatisfiesConstraints()
        {
            using (var sut = CreateSolver())
            {
                var actualResult = sut.Solve(MakeModel());
                var actualSnapshot = actualResult.Snapshot;
                var x = actualSnapshot.GetCompoundLabelByVariableName("x");
                Assert.That(x.GetValueAt(1), Is.EqualTo(x.GetValueAt(2)));
                Assert.That(x.GetValueAt(1), Is.EqualTo(x.GetValueAt(3)));
            }
        }

        [Test]
        public void SolveReturningLabelInValidRange()
        {
            using (var sut = CreateSolver())
            {
                var actualResult = sut.Solve(MakeModel());
                var actualSnapshot = actualResult.Snapshot;
                var xLabel = actualSnapshot.GetCompoundLabelByVariableName("x");
                Assert.That(xLabel.Values, Is.All.InRange(1, 10));
            }
        }

        private static OrangeSolver CreateSolver()
        {
            return new OrangeSolver();
        }

        private static ModelModel MakeModel()
        {
            var workspace = new WorkspaceBuilder("A multi-level repeater test")
                                          .AddAggregate("x", 10, "1..10")
                                          .WithConstraintExpression("$x[i] <> $x[j] + 1 | i,j in 0..9,0..i")
                                          .Build();

            return workspace.Model;
        }
    }
}
