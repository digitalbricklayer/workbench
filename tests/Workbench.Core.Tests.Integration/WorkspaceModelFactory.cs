using Workbench.Core.Models;

namespace Workbench.UI.Tests.Integration
{
    /// <summary>
    /// Factory for building workspace models.
    /// </summary>
    internal class WorkspaceModelFactory
    {
        private static ModelModel modelModel;

        /// <summary>
        /// Create a simple workspace model.
        /// </summary>
        /// <returns>A simple workspace model.</returns>
        internal static WorkspaceModel Create()
        {
            return new WorkspaceModel
            {
                Model = CreateModel(),
                Solution = CreateSolution()
            };
        }

        private static ModelModel CreateModel()
        {
            modelModel = new ModelModel();
            var x = new VariableGraphicModel("x", "z");
            modelModel.AddVariable(x);
            var y = new AggregateVariableGraphicModel("y", 10, new VariableDomainExpressionModel("1..9"));
            modelModel.AddVariable(y);
            var constraint = new ExpressionConstraintGraphicModel("X", "x > 1");
            modelModel.AddConstraint(constraint);
            var domain = new DomainGraphicModel("z", "1..10");
            modelModel.AddDomain(domain);

            return modelModel;
        }

        private static SolutionModel CreateSolution()
        {
            var solutionModel = new SolutionModel(modelModel);
            var x = new VariableGraphicModel("x");
            var valueOfX = new ValueModel(x, 1);
            solutionModel.Snapshot.AddSingletonValue(valueOfX);
            return solutionModel;
        }
    }
}
