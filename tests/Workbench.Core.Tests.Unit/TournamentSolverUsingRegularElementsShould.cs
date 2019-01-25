using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Test using the tournament schedule problem taken from here http://csplib.org/Problems/prob026/.
    /// </summary>
    [TestFixture]
    public class TournamentSolverUsingRegularElementsShould
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
            return new WorkspaceBuilder("Simple Tournament Schedule")
                            .WithSharedDomain("teams", "1..4")
                            .AddAggregate(variable =>
                                    {
                                        variable.WithName("week1");
                                        variable.WithSize(4);
                                        variable.WithDomain("$teams");
                                    })
                            .WithConstraintAllDifferent("week1")
                            .AddAggregate("week2", 4, "$teams")
                            .WithConstraintAllDifferent("week2")
                            .AddAggregate("week3", 4, "$teams")
                            .WithConstraintAllDifferent("week3")
                            .Build();
        }
    }
}
