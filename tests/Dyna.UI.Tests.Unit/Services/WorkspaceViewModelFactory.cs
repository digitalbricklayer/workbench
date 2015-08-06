using DynaApp.ViewModels;

namespace Dyna.UI.Tests.Unit.Services
{
    class WorkspaceViewModelFactory
    {
        internal static WorkspaceViewModel Create()
        {
            return new WorkspaceViewModel
            {
                Model = CreateModel(),
                Solution = CreateSolution()
            };
        }

        private static ModelViewModel CreateModel()
        {
            var modelViewModel = new ModelViewModel();
            var x = new VariableViewModel("x");
            modelViewModel.AddVariable(x);
            var constraint = new ConstraintViewModel("X", "x > 1");
            modelViewModel.AddConstraint(constraint);
            var domain = new DomainViewModel("z", "1..10");
            modelViewModel.AddDomain(domain);
            modelViewModel.Connect(x, constraint);
            modelViewModel.Connect(x, domain);

            return modelViewModel;
        }

        private static SolutionViewModel CreateSolution()
        {
            var solutionViewModel = new SolutionViewModel();
            var x = new VariableViewModel("x");
            var valueOfX = new ValueViewModel(x);
            solutionViewModel.AddValue(valueOfX);
            return solutionViewModel;
        }
    }
}
