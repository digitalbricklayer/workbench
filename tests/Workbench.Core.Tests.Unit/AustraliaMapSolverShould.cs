using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Test using the Australia map coloring problem.
    /// http://www.cs.colostate.edu/~asa/courses/cs440/fall09/pdfs/10_csp.pdf
    /// </summary>
    [TestFixture]
    public class AustraliaMapSolverShould
    {
        [Test]
        public void SolveWithAustraliaMapModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = WorkspaceModel.Create("Australia Map Coloring Model")
                                          .WithSharedDomain("colors", "1..3")
                                          .AddSingleton("wa", "colors")
                                          .AddSingleton("nt", "colors")
                                          .AddSingleton("sa", "colors")
                                          .AddSingleton("q", "colors")
                                          .AddSingleton("nsw", "colors")
                                          .AddSingleton("v", "colors")
                                          .AddSingleton("t", "colors")
                                          .WithConstraintExpression("wa <> nt")
                                          .WithConstraintExpression("wa <> sa")
                                          .WithConstraintExpression("nt <> q")
                                          .WithConstraintExpression("nt <> sa")
                                          .WithConstraintExpression("q <> nsw")
                                          .WithConstraintExpression("nsw <> sa")
                                          .WithConstraintExpression("nsw <> v")
                                          .WithConstraintExpression("sa <> v")
                                          .Build();

            return workspace;
        }
    }
}
