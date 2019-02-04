using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

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
        public void SolveWithTournamentModelAssignsBucketLabels()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Snapshot.BucketLabels, Is.Not.Empty);
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
