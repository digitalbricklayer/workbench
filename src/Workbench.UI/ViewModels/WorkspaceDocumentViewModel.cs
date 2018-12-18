using System;
using System.Diagnostics.Contracts;
using System.Windows;
using Caliburn.Micro;
using Microsoft.Win32;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Workspace document view model.
    /// </summary>
    public class WorkspaceDocumentViewModel : Screen, IHandle<WorkspaceChangedMessage>, IWorkspaceDocument
    {
        private IWorkspace _workspace;
        private DocumentPathViewModel _path;
        private bool _isDirty;
        private readonly IDataService _dataService;
        private bool _isNew = true;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWorkspaceLoader _workspaceLoader;

        /// <summary>
        /// Initialize the workspace document view model with a workspace.
        /// </summary>
        /// <param name="theWorkspaceViewModel">Workspace view model.</param>
        /// <param name="theDataService">Data service.</param>
        /// <param name="theEventAggregator">Event aggregator.</param>
        /// <param name="theWorkspaceLoader">Workspace loader.</param>
        public WorkspaceDocumentViewModel(IWorkspace theWorkspaceViewModel,
                                          IDataService theDataService,
                                          IEventAggregator theEventAggregator,
                                          IWorkspaceLoader theWorkspaceLoader)
        {
            Contract.Requires<ArgumentNullException>(theWorkspaceViewModel != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theWorkspaceLoader != null);

            Workspace = theWorkspaceViewModel;
            Path = new DocumentPathViewModel();
            _dataService = theDataService;
            _eventAggregator = theEventAggregator;
            _workspaceLoader = theWorkspaceLoader;
        }

        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        public IWorkspace Workspace
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
        /// Gets or sets the document path.
        /// </summary>
        public DocumentPathViewModel Path
        {
            get => _path;
            set
            {
                _path = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the document new flag.
        /// </summary>
        public bool IsNew
        {
            get => _isNew;
            set
            {
                _isNew = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the document dirty flag.
        /// </summary>
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Create a new workspace document.
        /// </summary>
        public void New()
        {
            IsNew = true;
            IsDirty = false;
        }

        /// <summary>
        /// Open the workspace document.
        /// </summary>
        public void Open()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Constraint Workbench" + " (*.dpf)|*.dpf|All Files|*.*",
                DefaultExt = "dpf",
                RestoreDirectory = true
            };

            // Show Open File dialog
            var openResult = openFileDialog.ShowDialog();

            if (!openResult.GetValueOrDefault())
            {
                // Open has been cancelled
                return;
            }

            Path = new DocumentPathViewModel(openFileDialog.FileName);
            DoLoad();
        }

        /// <summary>
        /// Close the document.
        /// </summary>
        /// <returns>True if the document was saved successfully, False if the
        /// save was cancelled by the user.</returns>
        public bool Close()
        {
            return TrySave();
        }

        /// <summary>
        /// Handle the "File|Save" menu item.
        /// </summary>
        public bool Save()
        {
            if (Path.IsEmpty)
            {
                var isCancelled = SaveAs();
                return isCancelled;
            }

            DoSave(Path.FullPath);
			return false;
        }

        /// <summary>
        /// Handle the "File|Save As" menu item.
        /// </summary>
        public bool SaveAs()
        {
            // Show Save File dialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Constraint Workbench" + " (*.dpf)|*.dpf|All Files|*.*",
                OverwritePrompt = true,
                DefaultExt = "dpf"
            };

            var result = saveFileDialog.ShowDialog();

            if (!result.GetValueOrDefault()) return true;

            Path = new DocumentPathViewModel(saveFileDialog.FileName);

            // Save
            return DoSave(saveFileDialog.FileName);
        }

        /// <summary>
        /// Handle the workspace changed message.
        /// </summary>
        /// <param name="message"></param>
        public void Handle(WorkspaceChangedMessage message)
        {
            DoDocumentChange();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _eventAggregator.Subscribe(this);
        }

        /// <summary>
        /// Prompt to save and make Save operation if necessary.
        /// </summary>
        /// <returns>
        /// true - caller can continue (open new file, close program etc.
        /// false - caller should cancel current operation.
        /// </returns>
        private bool TrySave()
        {
            if (!IsDirty)
            {
                // Nothing to save... file is up-to-date
                return true;
            }

            var result = MessageBox.Show(Application.Current.MainWindow,
                                         "Do you want to save changes?",
                                         "Constraint Workbench",
                                         MessageBoxButton.YesNoCancel,
                                         MessageBoxImage.Question,
                                         MessageBoxResult.Yes);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return Save();

                case MessageBoxResult.No:
                    // User wishes to discard changes
                    return true;

                case MessageBoxResult.Cancel:
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        /// Save the workspace document to a file.
        /// </summary>
        private bool DoSave(string file)
        {
            try
            {
                _dataService.Save(file);
            }
            catch (Exception e)
            {
                this.ShowError(e.Message);
                return false;
            }

            IsDirty = false;
            _eventAggregator.PublishOnUIThread(new DocumentSavedMessage(this));

            return true;
        }

        /// <summary>
        /// Show an error message.
        /// </summary>
        private void ShowError(string message)
        {
            MessageBox.Show(Application.Current.MainWindow,
                            message,
                            "Constraint Workbench",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        private void DoLoad()
        {
            try
            {
                // Read a new workspace model from file
                var newWorkspace = _dataService.Open(Path.FullPath);
                Workspace = _workspaceLoader.Load(newWorkspace);
                IsNew = false;
                IsDirty = false;
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }
        }

        private void DoDocumentChange()
        {
            IsNew = false;
            IsDirty = true;
        }
    }
}
