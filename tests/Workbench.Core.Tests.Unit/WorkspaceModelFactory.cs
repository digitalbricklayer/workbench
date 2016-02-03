using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Factory for building workspace models.
    /// </summary>
    internal class WorkspaceModelFactory
    {
        /// <summary>
        /// Create a simple workspace model.
        /// </summary>
        /// <returns>A simple workspace model.</returns>
        internal static WorkspaceModel Create()
        {
            return WorkspaceModel.Create()
                                 .AddSingleton("x", "z")
                                 .AddAggregate("y", 10, "1..9")
                                 .WithConstraint("x > 1")
                                 .WithSharedDomain("z", "1..10")
                                 .Build();
        }
    }
}
