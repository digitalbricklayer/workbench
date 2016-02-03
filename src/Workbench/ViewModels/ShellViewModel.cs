using System;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public sealed class ShellViewModel : Conductor<Screen>, IShell
    {
        private WorkspaceViewModel workspace;
        private ApplicationMenuViewModel applicationMenu;
        private readonly IAppRuntime appRuntime;
        private TitleBarViewModel titleBar;

        /// <summary>
        /// Initialize a shell view model with a data service and window manager.
        /// </summary>
        /// <param name="theAppRuntime">Application runtime.</param>
        /// <param name="theWorkspaceViewModel">Workspace view model.</param>
        /// <param name="theApplicationMenuViewModel">Application menu view model.</param>
        /// <param name="theTitleBarViewModel">Title bar view model.</param>
        public ShellViewModel(IAppRuntime theAppRuntime,
                              WorkspaceViewModel theWorkspaceViewModel,
                              ApplicationMenuViewModel theApplicationMenuViewModel,
                              TitleBarViewModel theTitleBarViewModel)
        {
            if (theAppRuntime == null)
                throw new ArgumentNullException("theAppRuntime");
            if (theWorkspaceViewModel == null)
                throw new ArgumentNullException("theWorkspaceViewModel");
            if (theApplicationMenuViewModel == null)
                throw new ArgumentNullException("theApplicationMenuViewModel");
            if (theTitleBarViewModel == null)
                throw new ArgumentNullException("theTitleBarViewModel");

            this.appRuntime = theAppRuntime;
            this.Workspace = theWorkspaceViewModel;
            this.appRuntime.Shell = this;
            this.ApplicationMenu = theApplicationMenuViewModel;
            this.TitleBar = theTitleBarViewModel;
            this.ActivateItem(this.Workspace);
        }

        /// <summary>
        /// Gets or sets the application menu.
        /// </summary>
        public ApplicationMenuViewModel ApplicationMenu
        {
            get { return this.applicationMenu; }
            set
            {
                this.applicationMenu = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        public WorkspaceViewModel Workspace
        {
            get { return this.workspace; }
            set
            {
                this.workspace = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the title bar view model.
        /// </summary>
        public TitleBarViewModel TitleBar
        {
            get { return this.titleBar; }
            set
            {
                this.titleBar = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
