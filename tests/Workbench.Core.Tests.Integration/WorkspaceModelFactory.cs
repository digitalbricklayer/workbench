using System;
using System.Collections.Generic;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.UI.Tests.Integration
{
    /// <summary>
    /// Factory for building workspace models.
    /// </summary>
    internal sealed class WorkspaceModelFactory
    {
        private ModelModel modelModel;

        /// <summary>
        /// Create a simple workspace model.
        /// </summary>
        /// <returns>A simple workspace model.</returns>
        internal static WorkspaceModel Create()
        {
            return new WorkspaceModelFactory().CreateWorkspace();
        }

        private WorkspaceModel CreateWorkspace()
        {
            return new WorkspaceModel
            {
                Model = CreateModel(),
                Solution = CreateSolution()
            };
        }

        private ModelModel CreateModel()
        {
            modelModel = new ModelModel();
            var x = new SingletonVariableGraphicModel(modelModel, "x", "z");
            modelModel.AddVariable(x);
            var y = new AggregateVariableGraphicModel(modelModel, "y", 10, new VariableDomainExpressionModel("1..9"));
            modelModel.AddVariable(y);
            var constraint = new ExpressionConstraintGraphicModel("X", "x > 1");
            modelModel.AddConstraint(constraint);
            var domain = new DomainGraphicModel("z", "1..10");
            modelModel.AddDomain(domain);

            return modelModel;
        }

        private SolutionModel CreateSolution()
        {
            var x = new SingletonVariableGraphicModel(modelModel, "x");
            var valueOfX = new ValueModel(x, new ValueBinding(1, 1));
            var snapshot = new SolutionSnapshot(Enumerable.Empty<ValueModel>(), new List<ValueModel> { valueOfX }, TimeSpan.Zero);
            var solutionModel = new SolutionModel(modelModel, snapshot);

            return solutionModel;
        }
    }
}
