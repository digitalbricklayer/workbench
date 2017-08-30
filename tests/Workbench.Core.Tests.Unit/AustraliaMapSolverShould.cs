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
#if false
                                          .WithVisualizerBinding("if <wa> = 1: states(row:1,column:2,BackgroundColor:red), if <wa> = 2: states(row:1,column:2,BackgroundColor:green), if <wa> = 3: states(row:1,column:2,BackgroundColor:blue)")
                                          .WithVisualizerBinding("if <nt> = 1: states(row:2,column:2,BackgroundColor:red), if <nt> = 2: states(row:2,column:2,BackgroundColor:green), if <nt> = 3: states(row:2,column:2,BackgroundColor:blue)")
                                          .WithVisualizerBinding("if <sa> = 1: states(row:3,column:2,BackgroundColor:red), if <sa> = 2: states(row:3,column:2,BackgroundColor:green), if <sa> = 3: states(row:3,column:2,BackgroundColor:blue)")
                                          .WithVisualizerBinding("if <q> = 1: states(row:4,column:2,BackgroundColor:red), if <q> = 2: states(row:4,column:2,BackgroundColor:green), if <q> = 3: states(row:4,column:2,BackgroundColor:blue)")
                                          .WithVisualizerBinding("if <nsw> = 1: states(row:5,column:2,BackgroundColor:red), if <nsw> = 2: states(row:5,column:2,BackgroundColor:green), if <nsw> = 3: states(row:5,column:2,BackgroundColor:blue)")
                                          .WithVisualizerBinding("if <v> = 1: states(row:6,column:2,BackgroundColor:red), if <v> = 2: states(row:6,column:2,BackgroundColor:green), if <v> = 3: states(row:6,column:2,BackgroundColor:blue)")
                                          .WithVisualizerBinding("if <t> = 1: states(row:7,column:2,BackgroundColor:red), if <t> = 2: states(row:7,column:2,BackgroundColor:green), if <t> = 3: states(row:7,column:2,BackgroundColor:blue)")
#else
                                          .WithVisualizerBinding("if <wa> = 1: states(row:1,column:2,Text:1), if <wa> = 2: states(row:1,column:2,Text:2), if <wa> = 3: states(row:1,column:2,Text:3)")
                                          .WithVisualizerBinding("if <nt> = 1: states(row:2,column:2,Text:1), if <nt> = 2: states(row:2,column:2,Text:2), if <nt> = 3: states(row:2,column:2,Text:3)")
                                          .WithVisualizerBinding("if <sa> = 1: states(row:3,column:2,Text:1), if <sa> = 2: states(row:3,column:2,Text:2), if <sa> = 3: states(row:3,column:2,Text:3)")
                                          .WithVisualizerBinding("if <q> = 1: states(row:4,column:2,Text:1), if <q> = 2: states(row:4,column:2,Text:2), if <q> = 3: states(row:4,column:2,Text:3)")
                                          .WithVisualizerBinding("if <nsw> = 1: states(row:5,column:2,Text:1), if <nsw> = 2: states(row:5,column:2,Text:2), if <nsw> = 3: states(row:5,column:2,Text:3)")
                                          .WithVisualizerBinding("if <v> = 1: states(row:6,column:2,Text:1), if <v> = 2: states(row:6,column:2,Text:2), if <v> = 3: states(row:6,column:2,Text:3)")
                                          .WithVisualizerBinding("if <t> = 1: states(row:7,column:2,Text:1), if <t> = 2: states(row:7,column:2,Text:2), if <t> = 3: states(row:7,column:2,Text:3)")
#endif
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
