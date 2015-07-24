using DynaApp.ViewModels;

namespace Dyna.UI.Tests.Unit.Services
{
    class WorkspaceViewModelFactory
    {
        internal static WorkspaceViewModel Create()
        {
            return new WorkspaceViewModel
            {
                Model = MakeModelViewModel(),
                Solution = MakeSolutionViewModel()
            };
        }

        private static ModelViewModel MakeModelViewModel()
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

        private static SolutionViewModel MakeSolutionViewModel()
        {
            var solutionViewModel = new SolutionViewModel();
            var x = new VariableViewModel("x");
            var valueOfX = new ValueViewModel(x);
            solutionViewModel.AddValue(valueOfX);
            return solutionViewModel;
        }
    }
}
