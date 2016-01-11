using System;
using System.IO;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public sealed class ShellViewModel : Screen, IShell
    {
        private string title = string.Empty;
        private WorkspaceViewModel workspace;
        private ApplicationMenuViewModel applicationMenu;
        private readonly IAppRuntime appRuntime;

        /// <summary>
        /// Initialize a shell view model with a data service and window manager.
        /// </summary>
        /// <param name="theAppRuntime">Application runtime.</param>
        /// <param name="theWorkspaceViewModel">Workspace view model.</param>
        /// <param name="theApplicationMenuViewModel">Application menu view model.</param>
        public ShellViewModel(IAppRuntime theAppRuntime, WorkspaceViewModel theWorkspaceViewModel, ApplicationMenuViewModel theApplicationMenuViewModel)
        {
            if (theAppRuntime == null)
                throw new ArgumentNullException("theAppRuntime");
            if (theWorkspaceViewModel == null)
                throw new ArgumentNullException("theWorkspaceViewModel");
            if (theApplicationMenuViewModel == null)
                throw new ArgumentNullException("theApplicationMenuViewModel");

            this.appRuntime = theAppRuntime;
            this.Workspace = theWorkspaceViewModel;
            this.appRuntime.Shell = this;
            this.ApplicationMenu = theApplicationMenuViewModel;
            this.UpdateTitle();
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
        /// Gets or sets the main window title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Update main window title.
        /// </summary>
        public void UpdateTitle()
        {
            var newTitle = this.appRuntime.ApplicationName + " - ";

            if (string.IsNullOrEmpty(this.appRuntime.CurrentFileName))
            {
                newTitle += "Untitled";
                this.Title = newTitle;
                return;
            }

            newTitle += Path.GetFileName(this.appRuntime.CurrentFileName);

            if (this.Workspace.IsDirty)
            {
                newTitle += " *";
            }

            this.Title = newTitle;
        }
    }
}
