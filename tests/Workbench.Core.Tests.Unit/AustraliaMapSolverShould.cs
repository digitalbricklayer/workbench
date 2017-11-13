using System.Linq;
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
        public void SolveWithGridVisualizerAssignsColorsWithinContraints()
        {
            var sut = CreateWorkspace();
            sut.Solve();
            var gridVisualizer = (GridVisualizerModel)sut.Solution.GetVisualizerBy("states");
            var colorColumnData = gridVisualizer.GetColumnDataByName("Color");
            var colorCells = colorColumnData.GetCells();
            Assert.That(colorCells, Has.Count.EqualTo(7), "There should be 7 cells in the color column corresponding to the 7 Australian states.");
            var waColor = colorCells.ElementAt(0);
            var ntColor = colorCells.ElementAt(1);
            Assert.That(waColor.Text, Is.Not.EqualTo(ntColor.Text), "WA and NT are adjacent states and should therefore not share the same color.");
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = WorkspaceModel.Create("Australia Map Coloring Model")
                                          .WithSharedDomain("colors", "red, green, blue")
                                          .AddSingleton("wa", "$colors")
                                          .AddSingleton("nt", "$colors")
                                          .AddSingleton("sa", "$colors")
                                          .AddSingleton("q", "$colors")
                                          .AddSingleton("nsw", "$colors")
                                          .AddSingleton("v", "$colors")
                                          .AddSingleton("t", "$colors")
                                          .WithConstraintExpression("wa <> nt")
                                          .WithConstraintExpression("wa <> sa")
                                          .WithConstraintExpression("nt <> q")
                                          .WithConstraintExpression("nt <> sa")
                                          .WithConstraintExpression("q <> nsw")
                                          .WithConstraintExpression("nsw <> sa")
                                          .WithConstraintExpression("nsw <> v")
                                          .WithConstraintExpression("sa <> v")
                                          .WithVisualizerBinding("states(row:1,column:2,Text:<wa>)")
                                          .WithVisualizerBinding("states(row:2,column:2,Text:<nt>)")
                                          .WithVisualizerBinding("states(row:3,column:2,Text:<sa>)")
                                          .WithVisualizerBinding("states(row:4,column:2,Text:<q>)")
                                          .WithVisualizerBinding("states(row:5,column:2,Text:<nsw>)")
                                          .WithVisualizerBinding("states(row:6,column:2,Text:<v>)")
                                          .WithVisualizerBinding("states(row:7,column:2,Text:<t>)")
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
