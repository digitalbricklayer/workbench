using Dyna.Core.Models;

namespace Dyna.UI.Tests.Integration
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
            return new WorkspaceModel
            {
                Model = CreateModel(),
                Solution = CreateSolution()
            };
        }

        private static ModelModel CreateModel()
        {
            var modelModel = new ModelModel();
            var x = new VariableModel("x", "z");
            modelModel.AddVariable(x);
            var y = new AggregateVariableModel("y", 10, new VariableDomainExpressionModel("1..9"));
            modelModel.AddVariable(y);
            var constraint = new ConstraintModel("X", "x > 1");
            modelModel.AddConstraint(constraint);
            var domain = new DomainModel("z", "1..10");
            modelModel.AddDomain(domain);

            return modelModel;
        }

        private static SolutionModel CreateSolution()
        {
            var solutionModel = new SolutionModel();
            var x = new VariableModel("x");
            var valueOfX = new ValueModel(x);
            solutionModel.AddSingletonValue(valueOfX);
            return solutionModel;
        }
    }
}
