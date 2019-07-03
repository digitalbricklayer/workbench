using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit.Solvers
{
    /// <summary>
    /// Simple tournament schedule test. Coping from the SimpleTournamentSolverShould
    /// fixture. Remove when the Orange solver is integrated into the model properly.
    /// </summary>
    [TestFixture]
    public class SimpleTournamentSolverShould
    {
        [Test]
        public void SolveWithTournamentModelReturnsStatusSuccess()
        {
            using (var sut = new OrangeSolver())
            {
                var actualResult = sut.Solve(CreateWorkspace().Model);
                Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
            }
        }

        [Test]
        public void SolveWithTournamentModelAssignsDifferentTeamToMatches()
        {
            using (var sut = new OrangeSolver())
            {
                var actualResult = sut.Solve(CreateWorkspace().Model);
                var weekBucketLabel = actualResult.Snapshot.GetBucketLabelByName("week");
                foreach (var matchLabel in weekBucketLabel.BundleLabels)
                {
                    var homeTeamLabel = matchLabel.GetSingletonLabelByName("home");
                    var awayTeamLabel = matchLabel.GetSingletonLabelByName("away");
                    Assert.That(homeTeamLabel.Value, Is.Not.EqualTo(awayTeamLabel.Value));
                }
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
