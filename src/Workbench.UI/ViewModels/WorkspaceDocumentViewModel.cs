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
    public class WorkspaceDocumentViewModel : Screen
    {
        private WorkspaceViewModel _workspace;
        private DocumentPathViewModel _path;
        private bool _isDirty;
        private readonly IDataService _dataService;
        private bool _isNew;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initialize the workspace document view model with a workspace.
        /// </summary>
        /// <param name="theWorkspaceViewModel">Workspace view model.</param>
        /// <param name="theDataService">Data service.</param>
        /// <param name="theEventAggregator">Event aggregator.</param>
        public WorkspaceDocumentViewModel(WorkspaceViewModel theWorkspaceViewModel, IDataService theDataService, IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theWorkspaceViewModel != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            Workspace = theWorkspaceViewModel;
            _dataService = theDataService;
            _eventAggregator = theEventAggregator;
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
            if (!TrySave()) return;
            IsNew = true;
            IsDirty = false;
            _eventAggregator.PublishOnUIThread(new DocumentCreatedMessage(this));
        }

        /// <summary>
        /// Open the workspace document.
        /// </summary>
        public void Open()
        {
            if (!TrySave()) return;

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Constraint Capers Workbench" + " (*.dpf)|*.dpf|All Files|*.*",
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

            try
            {
                // Read a new workspace model
                _dataService.Open(openFileDialog.FileName);
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }

            Path = new DocumentPathViewModel(openFileDialog.FileName);
            _eventAggregator.PublishOnUIThread(new DocumentOpenedMessage(this));
        }

        /// <summary>
        /// Handle the "File|Save" menu item.
        /// </summary>
        public void Save()
        {
            if (Path.IsEmpty)
            {
                SaveAs();
                return;
            }

            Save(Path.FullPath);
        }

        /// <summary>
        /// Handle the "File|Save As" menu item.
        /// </summary>
        public void SaveAs()
        {
            // Show Save File dialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Constraint Capers Workbench" + " (*.dpf)|*.dpf|All Files|*.*",
                OverwritePrompt = true,
                DefaultExt = "dpf",
                RestoreDirectory = true
            };

            var result = saveFileDialog.ShowDialog();

            if (!result.GetValueOrDefault()) return;

            // Save
            Save(saveFileDialog.FileName);
        }

        /// <summary>
        /// Prompt to save and make Save operation if necessary.
        /// </summary>
        /// <returns>
        /// true - caller can continue (open new file, close program etc.
        /// false - caller should cancel current operation.
        /// </returns>
        public bool TrySave()
        {
            if (!IsDirty)
            {
                // Nothing to save... file is up-to-date
                return true;
            }

            var result = MessageBox.Show(Application.Current.MainWindow,
                                         "Do you want to save changes?",
                                         "Constraint Capers Workbench",
                                         MessageBoxButton.YesNoCancel,
                                         MessageBoxImage.Question,
                                         MessageBoxResult.Yes);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return this.Save(Path.FullPath);

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
        private bool Save(string file)
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
                            "Constraint Capers Workbench",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }
}
