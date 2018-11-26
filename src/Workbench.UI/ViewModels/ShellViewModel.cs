using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private WorkspaceDocumentViewModel _currentDocument;
        private bool _isClosing;

        /// <summary>
        /// Initialize a shell view model with an application runtime, workspace view 
        /// model, application menu view model and title bar view model.
        /// </summary>
        /// <param name="theCurrentWorkspaceDocument">Current workspace document.</param>
        /// <param name="theApplicationMenuViewModel">Application menu view model.</param>
        public ShellViewModel(WorkspaceDocumentViewModel theCurrentWorkspaceDocument, ApplicationMenuViewModel theApplicationMenuViewModel)
        {
            Contract.Requires<ArgumentNullException>(theCurrentWorkspaceDocument != null);
            Contract.Requires<ArgumentNullException>(theApplicationMenuViewModel != null);

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

        public void Close()
        {
            _isClosing = true;
            Application.Current?.MainWindow?.Close();
        }

        /// <summary>
        /// Close the shell.
        /// </summary>
        public void OnClose(CancelEventArgs cancelEventArgs)
        {
            // The document is a new document with no changes or the user initiated application close, exit the application
            if (CurrentDocument.IsNew || _isClosing)
            {
                cancelEventArgs.Cancel = false;
                return;
            }

			// If the document has changes that have not been saved to the disk, ask the user if they wish to save the changes
            if (CurrentDocument.IsDirty)
            {
                do
                {
                    var result = MessageBox.Show("Would you like to save your changes?",
                                                 "Constraint Workbench",
                                                 MessageBoxButton.YesNoCancel);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            // The user wishes to save the document
                            var isCancelled = CurrentDocument.Save();
                            if (!isCancelled)
                            {
                                // The user cancelled the save, give the 3 options again
                                continue;
                            }

                            cancelEventArgs.Cancel = false;
                            return;

                        case MessageBoxResult.No:
                            // The user does not wish to save the document, exit the application
                            cancelEventArgs.Cancel = false;
                            return;

                        case MessageBoxResult.Cancel:
                            // The user selected the cancel option. Do not exit the application
                            cancelEventArgs.Cancel = true;
                            return;
                    }
                } while (true);
            }

            // If the document isn't dirty, exit the application
            cancelEventArgs.Cancel = false;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            CurrentDocument.New();
        }
    }
}
