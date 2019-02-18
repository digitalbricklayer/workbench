using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solvers;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Placeholder for a well known test that will use the table derived domain values successfully.
    /// </summary>
    public class TableSharedDomainSolverWithWholeColumnRangeShould
    {
        /// <summary>
        /// Gets or sets the test subject.
        /// </summary>
        public WorkspaceModel Subject { get; set; }

        [SetUp]
        public void Initialize()
        {
            Subject = CreateWorkspace();
        }

        [Test]
        public void SolveWithListModelReturnsStatusSuccess()
        {
            var actualResult = Subject.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithTableReturnsSnapshotWithValidResult()
        {
            var actualResult = Subject.Solve();
            var aLabel = actualResult.Snapshot.GetLabelByVariableName("a");
            Assert.That(aLabel.Value, Is.EqualTo("Bodie"));
        }

        private WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("A contrived table domain test")
                        .AddSingleton("a", "$workers")
                        .WithSharedDomain("workers", "workers!Name:Name")
                        .WithConstraintExpression("$a <> Morse")
                        .WithConstraintExpression("$a <> Lewis")
                        .WithConstraintExpression("$a <> Doyle")
                        .WithTable(CreateTableTab())
                        .Build();
        }

        private static TableTabModel CreateTableTab()
        {
            return new TableTabModel(CreateTable(), new WorkspaceTabTitle("Table containing the domain"));
        }

        private static TableModel CreateTable()
        {
            return new TableModel(new ModelName("workers"), CreateWorkerColumns(), CreateWorkerRows());
        }

        private static TableColumnModel[] CreateWorkerColumns()
        {
            return new []
            {
                new TableColumnModel("Name")
            };
        }

        private static TableRowModel[] CreateWorkerRows()
        {
            return new []
            {
                new TableRowModel("Morse"),
                new TableRowModel("Lewis"),
                new TableRowModel("Bodie"),
                new TableRowModel("Doyle"),
            };
        }
    }
}
