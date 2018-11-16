using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the shell of the main window.
    /// </summary>
    public sealed class ShellViewModel : Conductor<Screen>.Collection.AllActive, IShell
    {
        private ApplicationMenuViewModel _applicationMenu;
        private readonly IAppRuntime _appRuntime;
        private WorkspaceDocumentViewModel _currentDocument;

        /// <summary>
        /// Initialize a shell view model with an application runtime, workspace view 
        /// model, application menu view model and title bar view model.
        /// </summary>
        /// <param name="theAppRuntime">Application runtime.</param>
        /// <param name="theCurrentWorkspaceDocument">Current workspace document.</param>
        /// <param name="theApplicationMenuViewModel">Application menu view model.</param>
        public ShellViewModel(IAppRuntime theAppRuntime,
                              WorkspaceDocumentViewModel theCurrentWorkspaceDocument,
                              ApplicationMenuViewModel theApplicationMenuViewModel)
        {
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theCurrentWorkspaceDocument != null);
            Contract.Requires<ArgumentNullException>(theApplicationMenuViewModel != null);

            _appRuntime = theAppRuntime;
            _appRuntime.Shell = this;
            CurrentDocument = theCurrentWorkspaceDocument;
            ApplicationMenu = theApplicationMenuViewModel;
            var shellSubScreens = new List<Screen> { Workspace, ApplicationMenu, CurrentDocument };
            Items.AddRange(shellSubScreens);
        }

        /// <summary>
        /// Gets or sets the application menu.
        /// </summary>
        public ApplicationMenuViewModel ApplicationMenu
        {
            get => _applicationMenu;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _applicationMenu = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the current workspace document.
        /// </summary>
        public WorkspaceDocumentViewModel CurrentDocument
        {
            get => _currentDocument;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _currentDocument = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        public WorkspaceViewModel Workspace
        {
            get => CurrentDocument.Workspace;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                CurrentDocument.Workspace = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Close the shell.
        /// </summary>
        public void Close()
        {
            Application.Current?.MainWindow?.Close();
        }
    }
}
