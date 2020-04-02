using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Simple tournament schedule test using the all different constraint.
    /// </summary>
    [TestFixture]
    public class SimpleTournamentSolverUsingAllDifferentConstraintShould
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
            foreach (var matchLabel in weekBucketLabel.BundleLabels)
            {
                var homeTeamLabel = matchLabel.GetSingletonLabelByName("home");
                var awayTeamLabel = matchLabel.GetSingletonLabelByName("away");
                Assert.That(homeTeamLabel.Value, Is.Not.EqualTo(awayTeamLabel.Value));
            }
        }

        /// <summary>
        /// Create a workspace with a simple tournament model.
        /// </summary>
        /// <returns>A workspace with a tournament model.</returns>
        private static WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("Simple Tournament Schedule")
                        .WithSharedDomain("teams", "1..4")
                        .AddBundle(bundle =>
                        {
                            bundle.WithName("match");
                            bundle.AddSingleton("home", "$teams");
                            bundle.AddSingleton("away", "$teams");
                            bundle.WithAllDifferentConstraint("home, away");
                        })
                        .AddBundle(bundle =>
                        {
                            bundle.WithName("week");
                            bundle.AddBucket(bucket =>
                            {
                                bucket.WithName("matches");
                                bucket.WithSize(2);
                                bucket.WithContents("match");
                            });
                        })
                        .AddBucket(bucket =>
                        {
                            bucket.WithName("tournament");
                            bucket.WithSize(2);
                            bucket.WithContents("week");
                        })
                        .Build();
        }
    }
}
