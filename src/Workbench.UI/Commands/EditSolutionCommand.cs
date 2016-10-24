using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.ViewModels;

namespace Workbench.Commands
{
    public class EditSolutionCommand : CommandBase
    {
        private readonly IWindowManager windowManager;
        private readonly SolutionViewModel solution;

        public EditSolutionCommand(IWindowManager theWindowManager, WorkspaceViewModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            this.windowManager = theWindowManager;
            this.solution = theWorkspace.Solution;
        }

        public override void Execute(object parameter)
        {
            var solutionEditorViewModel = new SolutionEditorViewModel();
            solutionEditorViewModel.BindingExpression = this.solution.Model.Display.Binding.Text;
            var showDialogResult = this.windowManager.ShowDialog(solutionEditorViewModel);
            if (showDialogResult.HasValue && showDialogResult.Value)
            {
                this.solution.Model.Display.Binding.Text = solutionEditorViewModel.BindingExpression;
            }
        }
    }
}
