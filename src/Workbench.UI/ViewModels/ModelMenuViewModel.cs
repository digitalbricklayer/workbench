using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the 'Model' menu.
    /// </summary>
    public class ModelMenuViewModel
    {
        private readonly IAppRuntime appRuntime;
        private readonly TitleBarViewModel titleBar;

        public ModelMenuViewModel(IAppRuntime theAppRuntime, TitleBarViewModel theTitleBarViewModel)
        {
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);

            this.appRuntime = theAppRuntime;
            this.titleBar = theTitleBarViewModel;

            SolveCommand = new CommandHandler(ModelSolveAction);
        }

        /// <summary>
        /// Gets the Model|Solve command
        /// </summary>
        public ICommand SolveCommand { get; private set; }

        /// <summary>
        /// Gets the work area view model.
        /// </summary>
        public WorkspaceViewModel Workspace => this.appRuntime.Workspace;

        /// <summary>
        /// Solve the model.
        /// </summary>
        private void ModelSolveAction()
        {
            Workspace.SolveModel();
            this.titleBar.UpdateTitle();
        }
    }
}