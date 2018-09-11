using NUnit.Framework;
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
            var cLabel = actualResult.Snapshot.GetLabelByVariableName("c");
            var cValue = cLabel.Value;
            Assert.That(cValue, Is.InstanceOf<string>());
            Assert.That(cValue, Is.EqualTo("moon"));
        }

        [Test]
        public void SolveWithListModelReturnsValidAggregateValueBinding()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var aValue = actualResult.Snapshot.GetCompoundLabelByVariableName("a");
            Assert.That(aValue.Values, Is.Unique);
            Assert.That(aValue.Values, Is.All.TypeOf<string>());
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = new WorkspaceBuilder("A contrived list test")
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
