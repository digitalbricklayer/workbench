using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    internal class ContrivedTableSharedDomainWorkspaceBuilder
    {
        internal WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceBuilder("A contrived table domain test")
                .AddSingleton("a", "$a")
                .WithSharedDomain("a", "workers!Name1,Name2,Name3,Name4")
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
            return new[]
            {
                new TableColumnModel("Name")
            };
        }

        private static TableRowModel[] CreateWorkerRows()
        {
            return new[]
            {
                new TableRowModel("Morse"),
                new TableRowModel("Lewis"),
                new TableRowModel("Bodie"),
                new TableRowModel("Doyle"),
            };
        }
    }
}
