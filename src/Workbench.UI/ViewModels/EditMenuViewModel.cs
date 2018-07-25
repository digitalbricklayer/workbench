using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public sealed class EditMenuViewModel
    {
        private readonly IDataService dataService;
        private readonly WorkAreaMapper workAreaMapper;
        private readonly IAppRuntime appRuntime;
        private readonly TitleBarViewModel titleBar;

        public EditMenuViewModel(IDataService theDataService,
                                 WorkAreaMapper theWorkAreaMapper,
                                 IAppRuntime theAppRuntime,
                                 TitleBarViewModel theTitleBarViewModel)
        {
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theWorkAreaMapper != null);
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);

            this.dataService = theDataService;
            this.workAreaMapper = theWorkAreaMapper;
            this.appRuntime = theAppRuntime;
            this.titleBar = theTitleBarViewModel;
            DeleteSelectedCommand = new CommandHandler(DeleteSelectedAction, _ => CanDeleteSelectedExecute);
        }

        /// <summary>
        /// Gets the work area view model.
        /// </summary>
        public WorkspaceViewModel Workspace
        {
            get { return this.appRuntime.Workspace; }
        }

        /// <summary>
        /// Gets the Delete selected command.
        /// </summary>
        public ICommand DeleteSelectedCommand { get; private set; }

        /// <summary>
        /// Gets whether the "Model|Delete" menu item can be executed.
        /// </summary>
        public bool CanDeleteSelectedExecute
        {
            get
            {
                return Workspace.CanDeleteSelectedExecute();
            }
        }

        /// <summary>
        /// Delete all selected graphics.
        /// </summary>
        private void DeleteSelectedAction()
        {
            Workspace.DeleteSelectedGraphics();
        }
    }
}
