using System.Windows;
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
        public void SolveWithMapModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithGridVisualizerAssignsEightQueens()
        {
            var sut = CreateWorkspace();
            sut.Solve();
            var gridVisualizer = (GridVisualizerModel)sut.Solution.GetVisualizerBy("states");
            var colorColumn = gridVisualizer.GetColumnByName("Color");
            var colorCells = colorColumn.GetCells();
            Assert.That(colorCells, Has.Count.EqualTo(7));
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
                                          .WithGridVisualizer(CreateGrid())
                                          .Build();

            return workspace;
        }

        private static GridVisualizerModel CreateGrid()
        {
          return new GridVisualizerModel("states", new Point(), new[] { "Name", "Color" }, new[] { new GridRowModel("WA", ""),
                                                                                                   new GridRowModel("NT", ""),
                                                                                                   new GridRowModel("SA", ""),
                                                                                                   new GridRowModel("Q", ""),
                                                                                                   new GridRowModel("NSW", ""),
                                                                                                   new GridRowModel("V", ""),
                                                                                                   new GridRowModel("T", "") });
        }
    }
}
