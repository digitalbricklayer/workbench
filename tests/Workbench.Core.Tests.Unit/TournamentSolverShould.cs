using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Test using the tournament schedule problem taken from here http://csplib.org/Problems/prob026/.
    /// </summary>
    [TestFixture]
    public class TournamentSolverShould
    {
        [Test]
        public void SolveWithTournamentModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        /// <summary>
        /// Create a workspace with a simple tournament model.
        /// </summary>
        /// <returns>A workspace with a tournament model.</returns>
        private static WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("CSP Library Problem 26 Model")
							.WithSharedDomain("teams", "1..8")
                            .AddBucket("weeks", 7, "week")
                            .AddBundle("week")
                                .AddBucket("week", 4, "match")
							    .WithConstraintAllDifferent("week")
								.AddBundle("match")
                                    .AddSingleton("home", "teams")
								    .AddSingleton("away", "teams")
                                    .WithConstraintExpression("$home <> $away")
                            .Build();
        }
    }
}
