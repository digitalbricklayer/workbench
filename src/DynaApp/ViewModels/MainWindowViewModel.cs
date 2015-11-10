using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Dyna.Core.Models;
using DynaApp.Services;
using Microsoft.Win32;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public sealed class MainWindowViewModel : AbstractViewModel
    {
		private const string ProgramTitle = "Constraint Workbench";
        private string filename = string.Empty;
        private string title = string.Empty;
        private WorkspaceViewModel workspace;

        /// <summary>
        /// Initialize a main windows view model with default values.
        /// </summary>
        public MainWindowViewModel()
        {
            this.Workspace = new WorkspaceViewModel();
            this.UpdateTitle();
            this.CreateMenuCommands();
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
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets whether the "File|New" menu item can be executed.
        /// </summary>
        public bool CanFileNewExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets whether the "File|Save As" menu item can be executed.
        /// </summary>
        public bool CanFileSaveAsExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets whether the "File|Save" menu item can be executed.
        /// </summary>
        public bool CanFileSaveExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets whether the "File|Open" menu item can be executed.
        /// </summary>
        public bool CanFileOpenExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets whether the "File|Exit" menu item can be executed.
        /// </summary>
        public bool CanFileExitExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets whether the "Model|Solve" menu item can be executed.
        /// </summary>
        public bool CanModelSolveExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets whether the "Model|Add Constraint" menu item can be executed.
        /// </summary>
        public bool CanAddConstraintExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets whether the "Model|Add Singleton Variable" menu item can be executed.
        /// </summary>
        public bool CanAddSingletonVariableExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets whether the "Model|Add Aggregate Variable" menu item can be executed.
        /// </summary>
        public bool CanAddAggregateVariableExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets whether the "Model|Add Domain" menu item can be executed.
        /// </summary>
        public bool CanAddDomainExecute
        {
            get
            {
                // Can always execute
                return true;
            }
        }

        /// <summary>
        /// Gets the File|New command.
        /// </summary>
        public ICommand NewCommand { get; private set; }

        /// <summary>
        /// Gets the File|Open command.
        /// </summary>
        public ICommand OpenCommand { get; private set; }

        /// <summary>
        /// Gets the File|Save command.
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Gets the File|Save As command.
        /// </summary>
        public ICommand SaveAsCommand { get; private set; }

        /// <summary>
        /// Gets the File|Exit command.
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Solve command
        /// </summary>
        public ICommand SolveCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Add Singleton Variable command.
        /// </summary>
        public ICommand AddSingletonVariableCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Add Aggregate Variable command.
        /// </summary>
        public ICommand AddAggregateVariableCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Add Constraint command.
        /// </summary>
        public ICommand AddConstraintCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Add Domain command.
        /// </summary>
        public ICommand AddDomainCommand { get; private set; }

        /// <summary>
        /// Gets or sets the main window title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Handle the "File|New" menu item.
        /// </summary>
        private void FileNewAction()
        {
            if (!PromptToSave())
            {
                return;
            }

            this.Workspace.Reset();
            this.filename = string.Empty;
            this.Workspace.IsDirty = false;
            this.UpdateTitle();
        }

        /// <summary>
        /// Handle the "File|Open" menu item.
        /// </summary>
        private void FileOpenAction()
        {
            if (!PromptToSave()) return;

            // Show Open File dialog
            var openFileDialog = new OpenFileDialog
            {
                Filter = ProgramTitle + " (*.dpf)|*.dpf|All Files|*.*",
                DefaultExt = "dpf",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog().GetValueOrDefault() != true)
            {
                // Open has been cancelled
                return;
            }

            this.Workspace.Reset();

            try
            {
                // Load file
                var workspaceReader = new WorkspaceModelReader(openFileDialog.FileName);
                var theWorkspaceModel = workspaceReader.Read();
                var workspaceMapper = new WorkspaceMapper(new ModelViewModelCache());
                this.Workspace = workspaceMapper.MapFrom(theWorkspaceModel);
            }
            catch (Exception e)
            {
                this.ShowError(e.Message);
                return;
            }

            this.filename = openFileDialog.FileName;
            this.Workspace.IsDirty = false;
            this.UpdateTitle();
        }

        /// <summary>
        /// Handle the "File|Save" menu item.
        /// </summary>
        private void FileSaveAction()
        {
            if (string.IsNullOrEmpty(filename))
            {
                this.FileSaveAsAction();
                return;
            }

            this.Save(filename);
        }

        /// <summary>
        /// Handle the "File|Save As" menu item.
        /// </summary>
        private void FileSaveAsAction()
        {
            // Show Save File dialog
            var dlg = new SaveFileDialog
            {
                Filter = ProgramTitle + " (*.dpf)|*.dpf|All Files|*.*",
                OverwritePrompt = true,
                DefaultExt = "dpf",
                RestoreDirectory = true
            };

            if (dlg.ShowDialog().GetValueOrDefault() != true)
            {
                return;
            }

            // Save
            this.Save(dlg.FileName);
        }

        /// <summary>
        /// Handle the "File|Exit" menu item.
        /// </summary>
        private void FileExitAction()
        {
            if (!PromptToSave())
            {
                return;
            }

            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// Handle the "Model|Solve" menu item.
        /// </summary>
        private void ModelSolveAction()
        {
            this.Workspace.SolveModel(Application.Current.MainWindow);
            this.UpdateTitle();
        }

        /// <summary>
        /// Event raised to create a new singleton variable.
        /// </summary>
        private void ModelAddSingletonVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddSingletonVariable("New Variable", newVariableLocation);
            this.UpdateTitle();
        }

        /// <summary>
        /// Event raised to create a new singleton variable.
        /// </summary>
        private void ModelAddAggregateVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddAggregateVariable("New Variable", newVariableLocation);
            this.UpdateTitle();
        }

        /// <summary>
        /// Event raised to create a new constraint.
        /// </summary>
        private void ModelAddConstraintAction()
        {
            var newConstraintLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddConstraint("New Constraint", newConstraintLocation);
            this.UpdateTitle();
        }

        /// <summary>
        /// Event raised to create a new domain.
        /// </summary>
        private void ModelAddDomainAction()
        {
            var newDomainLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddDomain("New Domain", newDomainLocation);
            this.UpdateTitle();
        }

        /// <summary>
        /// Prompt to save and make Save operation if necessary.
        /// </summary>
        /// <returns>
        /// true - caller can continue (open new file, close program etc.
        /// false - caller should cancel current operation.
        /// </returns>
        private bool PromptToSave()
        {
            if (!this.Workspace.IsDirty)
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
                    return this.Save(this.filename);

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
        /// Save the workspace to a file.
        /// </summary>
        private bool Save(string file)
        {
            try
            {
                // Save file
                var workspaceWriter = new WorkspaceModelWriter(file);
                workspaceWriter.Write(this.Workspace.WorkspaceModel);
            }
            catch (Exception e)
            {
                this.ShowError(e.Message);
                return false;
            }

            this.filename = file;
            this.Workspace.IsDirty = false;
            UpdateTitle();

            return true;
        }

        /// <summary>
        /// Show an error message.
        /// </summary>
        private void ShowError(string message)
        {
            MessageBox.Show(Application.Current.MainWindow,
                            message,
                            ProgramTitle,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        /// <summary>
        /// Update main window title.
        /// </summary>
        private void UpdateTitle()
        {
            var newTitle = ProgramTitle + " - ";

            if (string.IsNullOrEmpty(this.filename))
            {
                newTitle += "Untitled";
                this.Title = newTitle;
                return;
            }

            newTitle += Path.GetFileName(this.filename);

            if (this.Workspace.IsDirty)
            {
                newTitle += " *";
            }

            this.Title = newTitle;
        }

        /// <summary>
        /// Create main menu commands.
        /// </summary>
        private void CreateMenuCommands()
        {
            this.NewCommand = new CommandHandler(FileNewAction, CanFileNewExecute);
            this.OpenCommand = new CommandHandler(FileOpenAction, CanFileOpenExecute);
            this.SaveCommand = new CommandHandler(FileSaveAction, CanFileSaveExecute);
            this.SaveAsCommand = new CommandHandler(FileSaveAsAction, CanFileSaveAsExecute);
            this.ExitCommand = new CommandHandler(FileExitAction, CanFileExitExecute);
            this.SolveCommand = new CommandHandler(ModelSolveAction, CanModelSolveExecute);
            this.AddSingletonVariableCommand = new CommandHandler(ModelAddSingletonVariableAction, CanAddSingletonVariableExecute);
            this.AddAggregateVariableCommand = new CommandHandler(ModelAddAggregateVariableAction, CanAddAggregateVariableExecute);
            this.AddConstraintCommand = new CommandHandler(ModelAddConstraintAction, CanAddConstraintExecute);
            this.AddDomainCommand = new CommandHandler(ModelAddDomainAction, CanAddDomainExecute);
        }
    }
}
