using NUnit.Framework;
using System;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit.Solver
{
    [TestFixture]
    public class OrToolsSolverExpressionTests
    {
        [Test]
        public void SolveWithExpressionModelReturnsStatusSuccess()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());

                Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
            }
        }

        [Test]
        public void SolveWithExpressionModelSatisfiesConstraints()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());

                var actualSnapshot = actualResult.Snapshot;
                var x = actualSnapshot.GetLabelByVariableName("x");
                var y = actualSnapshot.GetLabelByVariableName("y");
                Assert.That(x.GetValueAsInt() + 1, Is.Not.EqualTo(y.GetValueAsInt() - 1));
            }
        }

        [Test]
        public void SolveWithExpressionModelSolutionWithinDomain()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());

                var actualSnapshot = actualResult.Snapshot;
                var x = actualSnapshot.GetLabelByVariableName("x");
                var y = actualSnapshot.GetLabelByVariableName("y");
                Assert.That(x.Value, Is.InRange(1, 9));
                Assert.That(y.Value, Is.InRange(1, 9));
            }
        }

        [Test]
        public void SolveWithSimpleModelSolutionHasValidVariableCount()
        {
            using (var sut = new OrToolsSolver())
            {
                var actualResult = sut.Solve(MakeModel());

                var actualSnapshot = actualResult.Snapshot;
                var singletonVariableCount = actualSnapshot.SingletonValues.Count;
                Assert.That(singletonVariableCount, Is.EqualTo(3));
                Assert.That(actualSnapshot.AggregateValues, Is.Empty);
            }
        }

        private static ModelModel MakeModel()
        {
            var workspace = WorkspaceModel.Create("A test")
                                          .WithSharedDomain("a", "1..9")
                                          .AddSingleton("x", "$a")
                                          .AddSingleton("y", "$a")
                                          .AddSingleton("z", "$a")
                                          .WithConstraintExpression("$x + 1 != $y - 1")
                                          .WithConstraintExpression("$x <= $y")
                                          .WithConstraintExpression("$y = $z")
                                          .Build();

            return workspace.Model;
        }
    }
}
