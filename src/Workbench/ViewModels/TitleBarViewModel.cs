using System.IO;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    public class TitleBarViewModel : Screen
    {
        private string title = string.Empty;
        private readonly IAppRuntime appRuntime;

        public TitleBarViewModel(IAppRuntime theAppRuntime,
                                 WorkspaceViewModel theWorkspaceViewModel)
        {
            this.appRuntime = theAppRuntime;
            this.Workspace = theWorkspaceViewModel;
			this.UpdateTitle();
        }

        public WorkspaceViewModel Workspace { get; private set; }

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