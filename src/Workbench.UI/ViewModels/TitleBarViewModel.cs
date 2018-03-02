using System;
using System.Diagnostics.Contracts;
using System.IO;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    public class TitleBarViewModel : Screen
    {
        private string title = string.Empty;
        private readonly IAppRuntime appRuntime;

        public TitleBarViewModel(IAppRuntime theAppRuntime,
                                 WorkAreaViewModel theWorkAreaViewModel)
        {
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theWorkAreaViewModel != null);

            this.appRuntime = theAppRuntime;
            WorkArea = theWorkAreaViewModel;
			UpdateTitle();
        }

        public WorkAreaViewModel WorkArea { get; private set; }

        /// <summary>
        /// Gets or sets the main window title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
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

            if (this.WorkArea.IsDirty)
            {
                newTitle += " *";
            }

            this.Title = newTitle;
        }
    }
}