using System.Linq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Simple tournament schedule test.
    /// </summary>
    [TestFixture]
    public class SimpleTournamentSolverShould
    {
        [Test]
        public void SolveWithTournamentModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithTournamentModelAssignsDifferentTeamToMatches()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var weekBucketLabel = actualResult.Snapshot.GetBucketLabelByName("week");
            var firstMatchLabel = weekBucketLabel.BundleLabels.First();
            var homeTeamLabel = firstMatchLabel.GetSingletonLabelByName("home");
            var awayTeamLabel = firstMatchLabel.GetSingletonLabelByName("away");
            Assert.That(homeTeamLabel.Value, Is.Not.EqualTo(awayTeamLabel.Value));
        }

        /// <summary>
        /// Create a workspace with a simple tournament model.
        /// </summary>
        /// <returns>A workspace with a tournament model.</returns>
        private static WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("Simple Tournament Schedule")
                        .WithSharedDomain("teams", "1..4")
                        .WithConstraintExpression("%week[i].home <> %week[i].away | i in size(week)")
                        .AddBundle(bundle =>
                        {
                            bundle.WithName("match");
                            bundle.AddSingleton("home", "$teams");
                            bundle.AddSingleton("away", "$teams");
                        })
                        .AddBucket(bucket =>
                        {
                            bucket.WithName("week");
                            bucket.WithSize(4);
                            bucket.WithContents("match");
                        })
                        .Build();
        }
    }
}
