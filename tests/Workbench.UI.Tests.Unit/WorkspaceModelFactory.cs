using Workbench.Core.Models;

namespace Workbench.UI.Tests.Unit
{
    /// <summary>
    /// Factory for test workspace models.
    /// </summary>
    internal class WorkspaceModelFactory
    {
        /// <summary>
        /// Create a valid test workspace model.
        /// </summary>
        /// <returns>Workspace model with test data.</returns>
        internal static WorkspaceModel Create()
        {
            return WorkspaceModel.Create()
                                 .AddSingleton("x", "1..10")
                                 .AddAggregate("y", 10, "$z")
                                 .WithConstraintExpression("$x > 1")
                                 .WithSharedDomain("z", "1..10")
                                 .Build();
        }
    }
}
