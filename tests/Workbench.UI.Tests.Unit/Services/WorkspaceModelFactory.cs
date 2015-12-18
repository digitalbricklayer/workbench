using Workbench.Core.Models;

namespace Workbench.UI.Tests.Unit.Services
{
    /// <summary>
    /// Factory for test workspace models.
    /// </summary>
    class WorkspaceModelFactory
    {
        private ModelModel model;

        /// <summary>
        /// Create a test workspace model.
        /// </summary>
        /// <returns>Workspace model with test data.</returns>
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
            this.model = new ModelModel();
            var x = new VariableModel("x");
            this.model.AddVariable(x);
            var constraint = new ConstraintModel("X", "x > 1");
            this.model.AddConstraint(constraint);
            var domain = new DomainModel("z", "1..10");
            this.model.AddDomain(domain);

            return this.model;
        }

        private SolutionModel CreateSolution()
        {
            var solution = new SolutionModel();
            var x = this.model.GetVariableByName("x");
            var valueOfX = new ValueModel(x);
            solution.AddSingletonValue(valueOfX);

            return solution;
        }
    }
}
