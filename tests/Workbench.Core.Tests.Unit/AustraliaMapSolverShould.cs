using System.Linq;
using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

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
        public void SolveWithMapModelReturnsValidSnapshot()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var waValue = actualResult.Snapshot.GetLabelByVariableName("wa");
            Assert.That(waValue.Value, Is.TypeOf<string>());
        }

        [Test]
        public void SolveWithTableVisualizerAssignsColorsWithinConstraints()
        {
            var sut = CreateWorkspace();
            sut.Solve();
            var tableVisualizer = (TableTabModel)sut.GetVisualizerBy("states");
            var colorColumnData = tableVisualizer.GetColumnDataByName("Color");
            var colorCells = colorColumnData.GetCells();
            Assert.That(colorCells, Has.Count.EqualTo(7), "There should be 7 cells in the color column corresponding to the 7 Australian states.");
            var waColor = colorCells.ElementAt(0);
            var ntColor = colorCells.ElementAt(1);
            Assert.That(waColor.Text, Is.Not.Empty);
            Assert.That(waColor.Text, Is.Not.EqualTo(ntColor.Text), "WA and NT are adjacent states and should therefore not share the same color.");
        }

        private static WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("Australia Map Coloring Model")
                        .WithSharedDomain("colors", "\"red\", \"green\", \"blue\"")
                        .AddSingleton("wa", "$colors")
                        .AddSingleton("nt", "$colors")
                        .AddSingleton("sa", "$colors")
                        .AddSingleton("q", "$colors")
                        .AddSingleton("nsw", "$colors")
                        .AddSingleton("v", "$colors")
                        .AddSingleton("t", "$colors")
                        .WithConstraintExpression("$wa <> $nt")
                        .WithConstraintExpression("$wa <> $sa")
                        .WithConstraintExpression("$nt <> $q")
                        .WithConstraintExpression("$nt <> $sa")
                        .WithConstraintExpression("$q <> $nsw")
                        .WithConstraintExpression("$nsw <> $sa")
                        .WithConstraintExpression("$nsw <> $v")
                        .WithConstraintExpression("$sa <> $v")
                        .WithTable(CreateTable())
                        .Build();
        }

        private static TableTabModel CreateTable()
        {
            var newTable = new TableModel(new ModelName("states"), new[] { "Text", "Color" }, CreateTableRows());
            return new TableTabModel(newTable, new WorkspaceTabTitle("Australian States"));
        }

        private static TableRowModel[] CreateTableRows()
        {
            return new[]
            {
                new TableRowModel(new TableCellModel("WA"), new TableCellModel("", "<wa>")),
                new TableRowModel(new TableCellModel("NT"), new TableCellModel("", "<nt>")),
                new TableRowModel(new TableCellModel("SA"), new TableCellModel("", "<sa>")),
                new TableRowModel(new TableCellModel("Q"), new TableCellModel("", "<q>")),
                new TableRowModel(new TableCellModel("NSW"), new TableCellModel("", "<nsw>")),
                new TableRowModel(new TableCellModel("V"), new TableCellModel("", "<v>")),
                new TableRowModel(new TableCellModel("T"), new TableCellModel("", "<t>")),
            };
        }
    }
}
