using NUnit.Framework;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Placeholder for a well known test that will use the list range successfully.
    /// </summary>
    [TestFixture]
    public class ListSolverShould
    {
        [Test]
        public void SolveWithListModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithListModelReturnsValidSingletonValueBinding()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var cValue = actualResult.Snapshot.GetSingletonVariableValueByName("c");
            var cValueBinding = cValue.GetBindingAt(0);
            Assert.That(cValueBinding.Solver, Is.InRange(1, 3));
            Assert.That(cValueBinding.Model, Is.InstanceOf<string>());
            Assert.That(cValueBinding.Model, Is.EqualTo("moon"));
        }

        [Test]
        public void SolveWithListModelReturnsValidAggregateValueBinding()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var aValue = actualResult.Snapshot.GetAggregateVariableValueByName("a");
            var actualSolverValues = aValue.Bindings.Select(_ => _.Solver)
                                                    .ToArray();
            var actualModelValues = aValue.Bindings.Select(_ => _.Model)
                                                   .ToArray();
            Assert.That(aValue.Values, Is.Unique);
            Assert.That(aValue.Values, Is.All.TypeOf<string>());
            Assert.That(actualSolverValues, Is.All.InRange(1, 3));
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = WorkspaceModel.Create("A contrived list test")
                                          .AddAggregate("a", 3, "bob, jim, kate")
                                          .AddSingleton("c", "sun, moon, sky")
                                          .WithConstraintAllDifferent("a")
                                          .WithConstraintExpression("$c <> sun")
                                          .WithConstraintExpression("$c <> sky")
                                          .Build();

            return workspace;
        }
    }
}
