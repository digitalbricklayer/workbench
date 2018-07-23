using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the 'Model' menu.
    /// </summary>
    public class ModelMenuViewModel
    {
        private readonly IWindowManager windowManager;
        private readonly IAppRuntime appRuntime;
        private readonly TitleBarViewModel titleBar;

        public ModelMenuViewModel(IWindowManager theWindowManager,
                                  IAppRuntime theAppRuntime,
                                  TitleBarViewModel theTitleBarViewModel)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);

            this.windowManager = theWindowManager;
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
        public WorkAreaViewModel WorkArea
        {
            get { return this.appRuntime.WorkArea; }
        }

        /// <summary>
        /// Solve the model.
        /// </summary>
        private void ModelSolveAction()
        {
            this.WorkArea.SolveModel();
            this.titleBar.UpdateTitle();
        }
    }
}