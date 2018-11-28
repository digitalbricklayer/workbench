using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows;
using Caliburn.Micro;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the shell of the main window.
    /// </summary>
    public sealed class ShellViewModel : Conductor<Screen>.Collection.AllActive, IShell
    {
        private readonly IDocumentManager _documentManager;
        private ApplicationMenuViewModel _applicationMenu;
        private WorkspaceDocumentViewModel _currentDocument;
        private WorkspaceViewModel _workspace;
        private bool _isClosing;

        /// <summary>
        /// Initialize a shell view model with an application runtime, workspace view 
        /// model, application menu view model and title bar view model.
        /// </summary>
        /// <param name="theDocumentManager">Document manager.</param>
        /// <param name="theApplicationMenuViewModel">Application menu view model.</param>
        public ShellViewModel(IDocumentManager theDocumentManager, ApplicationMenuViewModel theApplicationMenuViewModel)
        {
            Contract.Requires<ArgumentNullException>(theDocumentManager != null);
            Contract.Requires<ArgumentNullException>(theApplicationMenuViewModel != null);

            _documentManager = theDocumentManager;
            ApplicationMenu = theApplicationMenuViewModel;
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
            get => _workspace;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _workspace = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Close the application.
        /// </summary>
        public void Close()
        {
            _isClosing = true;
            // Closing the main window will raise the closing event for the main application window.
            Application.Current?.MainWindow?.Close();
        }

        public void OpenDocument(WorkspaceDocumentViewModel theDocument)
        {
            // Close the old document
            DeactivateItem(CurrentDocument, close: true);
            DeactivateItem(Workspace, close: true);
            CurrentDocument = theDocument;
            ActivateItem(CurrentDocument);
            Workspace = theDocument.Workspace;
            ActivateItem(Workspace);
        }

        /// <summary>
        /// Handler for the shell closing event.
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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            CurrentDocument = _documentManager.CreateDocument();
            Workspace = CurrentDocument.Workspace;
            var shellSubScreens = new List<Screen> { Workspace, ApplicationMenu, CurrentDocument };
            Items.AddRange(shellSubScreens);
            CurrentDocument.New();
        }
    }
}
