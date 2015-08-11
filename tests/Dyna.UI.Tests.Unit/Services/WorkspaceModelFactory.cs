using DynaApp.Models;

namespace Dyna.UI.Tests.Unit.Services
{
    /// <summary>
    /// Factory for test workspace models.
    /// </summary>
    class WorkspaceModelFactory
    {
        /// <summary>
        /// Create a test workspace model.
        /// </summary>
        /// <returns>Workspace model with test data.</returns>
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
            var model = new ModelModel();
            var x = new VariableModel("x");
            model.AddVariable(x);
            var constraint = new ConstraintModel("X", "x > 1");
            model.AddConstraint(constraint);
            var domain = new DomainModel("z", "1..10");
            model.AddDomain(domain);
            model.Connect(x, constraint);
            model.Connect(x, domain);

            return model;
        }

        private static SolutionModel CreateSolution()
        {
            var solution = new SolutionModel();
            var x = new VariableModel("x");
            var valueOfX = new ValueModel(x);
            solution.AddValue(valueOfX);

            return solution;
        }
    }
}
