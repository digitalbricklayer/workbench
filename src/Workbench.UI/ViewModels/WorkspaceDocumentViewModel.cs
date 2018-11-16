using System;
using System.Diagnostics.Contracts;
using System.Windows;
using Caliburn.Micro;
using Microsoft.Win32;
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
        private readonly IAppRuntime _appRuntime;
        private readonly IDataService _dataService;
        private readonly TitleBarViewModel _titleBar;

        /// <summary>
        /// Initialize the workspace document view model with a workspace.
        /// </summary>
        /// <param name="theWorkspaceViewModel">Workspace view model.</param>
        public WorkspaceDocumentViewModel(WorkspaceViewModel theWorkspaceViewModel,
                                          IAppRuntime theAppRuntime,
                                          IDataService theDataService,
                                          TitleBarViewModel theTitleBar)
        {
            Contract.Requires<ArgumentNullException>(theWorkspaceViewModel != null);
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theTitleBar != null);

            Workspace = theWorkspaceViewModel;
            _appRuntime = theAppRuntime;
            _dataService = theDataService;
            _titleBar = theTitleBar;
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
        /// Gets or sets the work area dirty flag.
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
            _appRuntime.CurrentDocument = this;
            Workspace = _appRuntime.Workspace;
            _titleBar.UpdateTitle();
        }

        /// <summary>
        /// Handle the "File|Open" menu item.
        /// </summary>
        public void Open()
        {
            if (!TrySave()) return;

            var openFileDialog = new OpenFileDialog
            {
                Filter = _appRuntime.ApplicationName + " (*.dpf)|*.dpf|All Files|*.*",
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
                _appRuntime.CurrentDocument = this;
                Workspace = _appRuntime.Workspace;
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }

            _appRuntime.CurrentFileName = openFileDialog.FileName;
            _titleBar.UpdateTitle();
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

            Save(_appRuntime.CurrentFileName);
        }

        /// <summary>
        /// Handle the "File|Save As" menu item.
        /// </summary>
        public void SaveAs()
        {
            // Show Save File dialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = _appRuntime.ApplicationName + " (*.dpf)|*.dpf|All Files|*.*",
                OverwritePrompt = true,
                DefaultExt = "dpf",
                RestoreDirectory = true
            };

            var result = saveFileDialog.ShowDialog();

            if (result.GetValueOrDefault() != true)
                return;

            // Save
            this.Save(saveFileDialog.FileName);
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
                                         "Dyna",
                                         MessageBoxButton.YesNoCancel,
                                         MessageBoxImage.Question,
                                         MessageBoxResult.Yes);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return this.Save(_appRuntime.CurrentFileName);

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

            _appRuntime.CurrentFileName = file;
            _appRuntime.CurrentDocument.IsDirty = false;
            _titleBar.UpdateTitle();

            return true;
        }

        /// <summary>
        /// Show an error message.
        /// </summary>
        private void ShowError(string message)
        {
            MessageBox.Show(Application.Current.MainWindow,
                            message,
                            _appRuntime.ApplicationName,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }
}
