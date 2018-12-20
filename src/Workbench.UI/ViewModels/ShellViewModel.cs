using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows;
using Caliburn.Micro;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the shell inside the main window. The shell is responsible
    /// for managing the single document interface.
    /// </summary>
    public sealed class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDocumentManager _documentManager;
        private IApplicationMenu _applicationMenu;
        private IWorkspaceDocument _currentDocument;
        private IWorkspace _workspace;
        private bool _isClosing;

        /// <summary>
        /// Initialize a shell view model with a document manager, application menu view model and event aggregator.
        /// </summary>
        /// <param name="theDocumentManager">Document manager.</param>
        /// <param name="theApplicationMenuViewModel">Application menu view model.</param>
        /// <param name="theEventAggregator">Event aggregator.</param>
        public ShellViewModel(IDocumentManager theDocumentManager, IApplicationMenu theApplicationMenuViewModel, IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theDocumentManager != null);
            Contract.Requires<ArgumentNullException>(theApplicationMenuViewModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            _documentManager = theDocumentManager;
            ApplicationMenu = theApplicationMenuViewModel;
            _eventAggregator = theEventAggregator;
        }

        /// <summary>
        /// Gets or sets the application menu.
        /// </summary>
        public IApplicationMenu ApplicationMenu
        {
            get => _applicationMenu;
            set
            {
                _applicationMenu = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the current workspace document.
        /// </summary>
        public IWorkspaceDocument CurrentDocument
        {
            get => _currentDocument;
            set
            {
                _currentDocument = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        public IWorkspace Workspace
        {
            get => _workspace;
            set
            {
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

        /// <summary>
        /// Open the workspace document.
        /// </summary>
        /// <param name="theDocument">Workspace document.</param>
        public void OpenDocument(IWorkspaceDocument theDocument)
        {
            CurrentDocument = theDocument;
            ActivateItem(CurrentDocument);
            Workspace = theDocument.Workspace;
            ActivateItem(Workspace);
            _eventAggregator.PublishOnUIThread(new DocumentOpenedMessage(CurrentDocument));
        }

        /// <summary>
        /// Close the current document.
        /// </summary>
        /// <returns>True if the document was successfully closed, false if cancelled.</returns>
        public bool CloseDocument()
        {
            var closeStatus = CurrentDocument.Close();
            // Did the user cancel the close?
            if (!closeStatus) return false;
            DeactivateItem(CurrentDocument, close: true);
            DeactivateItem(Workspace, close: true);
            _eventAggregator.PublishOnUIThread(new DocumentClosedMessage(CurrentDocument));
            CurrentDocument = null;
            return true;
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
            Items.Add(ApplicationMenu);
            var newDocument = _documentManager.CreateDocument();
            newDocument.New();
            OpenDocument(newDocument);
            _eventAggregator.Subscribe(this);
        }
    }
}
