using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Placeholder for a well known test that will use the character range successfully.
    /// </summary>
    [TestFixture, Timeout(1000)]
    public class CharacterSolverShould
    {
        [Test]
        public void SolveWithCharacterModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithCharacterModelReturnsValidSnapshot()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var aLabel = actualResult.Snapshot.GetAggregateLabelByVariableName("a");
            Assert.That(aLabel.Values, Is.Unique);
            Assert.That(aLabel.Values, Is.All.TypeOf<char>()
                                                       .And
                                                       .InRange('a', 'z').Using(new CharacterRangeComparer()));
        }

        [Test]
        public void SolveWithCharacterModelReturnsValidCharacterModelBinding()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var bLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("b");
            Assert.That(bLabel.Value, Is.InRange('a', 'z')
                                                  .Using(new CharacterRangeComparer()));
        }

        [Test]
        public void SolveWithCharacterModelReturnsValidbLabel()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var bLabel = actualResult.Snapshot.GetSingletonLabelByVariableName("b");
            Assert.That(bLabel.Value, Is.Not.EqualTo('a'));
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = new WorkspaceBuilder("A made up character range test")
                                          .AddAggregate("a", 26, "'a'..'z'")
                                          .AddSingleton("b", "'a'..'z'")
                                          .WithConstraintAllDifferent("a")
                                          .WithConstraintExpression("$b <> 'a'")
                                          .Build();

            return workspace;
        }
    }
}
