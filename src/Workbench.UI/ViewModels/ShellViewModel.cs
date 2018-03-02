using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the main window shell.
    /// </summary>
    public sealed class ShellViewModel : Conductor<Screen>, IShell
    {
        private WorkAreaViewModel workArea;
        private ApplicationMenuViewModel applicationMenu;
        private readonly IAppRuntime appRuntime;
        private TitleBarViewModel titleBar;

        /// <summary>
        /// Initialize a shell view model with an application runtime, workspace view 
        /// model, application menu view model and title bar view model.
        /// </summary>
        /// <param name="theAppRuntime">Application runtime.</param>
        /// <param name="theWorkspaceViewModel">Workspace view model.</param>
        /// <param name="theApplicationMenuViewModel">Application menu view model.</param>
        /// <param name="theTitleBarViewModel">Title bar view model.</param>
        public ShellViewModel(IAppRuntime theAppRuntime,
                              WorkAreaViewModel theWorkspaceViewModel,
                              ApplicationMenuViewModel theApplicationMenuViewModel,
                              TitleBarViewModel theTitleBarViewModel)
        {
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theWorkspaceViewModel != null);
            Contract.Requires<ArgumentNullException>(theApplicationMenuViewModel != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);

            this.appRuntime = theAppRuntime;
            this.appRuntime.Shell = this;
            WorkArea = theWorkspaceViewModel;
            ApplicationMenu = theApplicationMenuViewModel;
            TitleBar = theTitleBarViewModel;
        }

        /// <summary>
        /// Gets or sets the application menu.
        /// </summary>
        public ApplicationMenuViewModel ApplicationMenu
        {
            get { return this.applicationMenu; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.applicationMenu = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        public WorkAreaViewModel WorkArea
        {
            get { return this.workArea; }
            set
            {
                this.workArea = value;
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

        protected override void OnActivate()
        {
            base.OnActivate();
            ActivateItem(WorkArea);
            ActivateItem(ApplicationMenu);
            ActivateItem(TitleBar);
        }
    }
}
