using System;
using System.Collections.Generic;
using System.Linq;
using Workbench.Core;
using Workbench.Core.Models;

namespace Workbench.UI.Tests.Integration
{
    /// <summary>
    /// Factory for building workspace models.
    /// </summary>
    internal sealed class WorkspaceModelFactory
    {
        /// <summary>
        /// Create a simple workspace model.
        /// </summary>
        /// <returns>A simple workspace model.</returns>
        internal WorkspaceModel Create()
        {
            var newWorkspace = new WorkspaceBuilder("An example model for no particular purpose.")
                                             .AddSingleton("x", "z")
                                             .AddAggregate("y", 10, "1..9")
                                             .WithConstraintExpression("x > 1")
                                             .WithSharedDomain("z", "1..10")
                                             .Build();
            var xVariable = newWorkspace.Model.GetVariableByName("x");
            var valueOfX = new SingletonLabelModel((SingletonVariableModel) xVariable, new ValueModel(1));
            var snapshot = new SolutionSnapshot(new List<SingletonLabelModel> { valueOfX }, Enumerable.Empty<CompoundLabelModel>(), TimeSpan.Zero);
            newWorkspace.Solution = new SolutionModel(newWorkspace.Model, snapshot);

            return newWorkspace;
        }
    }
}
