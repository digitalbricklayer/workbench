using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

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
            var cLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("c");
            Assert.That(cLabel.Value, Is.InstanceOf<string>().And.EqualTo("moon"));
        }

        [Test]
        public void SolveWithListModelReturnsValidAggregateLabelValues()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var aLabel = actualResult.Snapshot.GetAggregateLabelByVariableName("a");
            Assert.That(aLabel.Values, Is.Unique);
            Assert.That(aLabel.Values, Is.All.TypeOf<string>());
        }

        private static WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("A contrived list test")
                            .AddAggregate("a", 3, "\"bob\", \"jim\", \"kate\"")
                            .AddSingleton("c", "\"sun\", \"moon\", \"sky\"")
                            .WithConstraintAllDifferent("a")
                            .WithConstraintExpression("$c <> sun")
                            .WithConstraintExpression("$c <> sky")
                            .Build();
        }
    }
}
