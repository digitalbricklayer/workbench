using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Microsoft.Win32;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public sealed class ShellViewModel : Screen, IShell
    {
		private const string ProgramTitle = "Constraint Capers Workbench";
        private string title = string.Empty;
        private WorkspaceViewModel workspace;
        private readonly IDataService dataService;
        private readonly IWindowManager windowManager;
        private string fileName = string.Empty;

        /// <summary>
        /// Initialize a shell view model with a data service and window manager.
        /// </summary>
        /// <param name="theDataService">Data service.</param>
        /// <param name="theWindowManager">Window manager.</param>
        public ShellViewModel(IDataService theDataService, IWindowManager theWindowManager)
        {
            if (theDataService == null)
                throw new ArgumentNullException("theDataService");
            if (theWindowManager == null)
                throw new ArgumentNullException("theWindowManager");

            this.dataService = theDataService;
            this.windowManager = theWindowManager;
            this.Workspace = IoC.Get<WorkspaceViewModel>();
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
                NotifyOfPropertyChange();
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
        /// Gets whether the "Model|Delete" menu item can be executed.
        /// </summary>
        public bool CanDeleteExecute
        {
            get
            {
                return this.Workspace.Model.Items.Any(_ => _.IsSelected);
            }
        }

        /// <summary>
        /// Gets whether the "Model|Resize" menu item can be executed.
        /// </summary>
        public bool CanResizeExecute
        {
            get
            {
                return this.Workspace.Model.GetSelectedAggregateVariables().Any();
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
        /// Gets the Model|Delete command.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Resize command.
        /// </summary>
        public ICommand ResizeCommand { get; private set; }

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
        /// Handle the "File|New" menu item.
        /// </summary>
        private void FileNewAction()
        {
            if (!PromptToSave()) return;
            this.Workspace.Reset();
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
                var workspaceModel = this.dataService.Open(openFileDialog.FileName);
                var workspaceMapper = new WorkspaceMapper(this.windowManager, null);
                this.Workspace = workspaceMapper.MapFrom(workspaceModel);
                this.Workspace.SelectedDisplayMode = "Model";
            }
            catch (Exception e)
            {
                this.ShowError(e.Message);
            }

            this.fileName = openFileDialog.FileName;
            this.Workspace.IsDirty = false;
            this.UpdateTitle();
        }

        /// <summary>
        /// Handle the "File|Save" menu item.
        /// </summary>
        private void FileSaveAction()
        {
            if (string.IsNullOrEmpty(this.fileName))
            {
                this.FileSaveAsAction();
                return;
            }

            this.Save(this.fileName);
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
        /// Solve the model.
        /// </summary>
        private void ModelSolveAction()
        {
            this.Workspace.SolveModel();
            this.UpdateTitle();
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void ModelAddSingletonVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddSingletonVariable("New Variable", newVariableLocation);
            this.UpdateTitle();
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void ModelAddAggregateVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddAggregateVariable("New Variable", newVariableLocation);
            this.UpdateTitle();
        }

        /// <summary>
        /// Create a new constraint.
        /// </summary>
        private void ModelAddConstraintAction()
        {
            var newConstraintLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddConstraint("New Constraint", newConstraintLocation);
            this.UpdateTitle();
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        private void ModelAddDomainAction()
        {
            var newDomainLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddDomain("New Domain", newDomainLocation);
            this.UpdateTitle();
        }

        /// <summary>
        /// Delete all selected graphics.
        /// </summary>
        private void ModelDeleteAction()
        {
            this.Workspace.DeleteSelectedGraphics();
            this.UpdateTitle();
        }

        /// <summary>
        /// Resize the selected aggregate variable.
        /// </summary>
        private void ModelResizeAction()
        {
            var selectedVariable = this.Workspace.Model.GetSelectedAggregateVariables();
            if (selectedVariable == null) return;

            var resizeViewModel = new AggregateVariableResizeViewModel();
            var showDialogResult = this.windowManager.ShowDialog(resizeViewModel);

            if (showDialogResult.HasValue && showDialogResult.Value)
            {
                foreach (var variableViewModel in selectedVariable)
                {
                    var aggregate = (AggregateVariableViewModel) variableViewModel;
                    aggregate.NumberVariables = Convert.ToString(resizeViewModel.Size);
                }
            }
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
                    return this.Save(this.fileName);

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
                this.dataService.Save(file);
            }
            catch (Exception e)
            {
                this.ShowError(e.Message);
                return false;
            }

            this.fileName = file;
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

            if (string.IsNullOrEmpty(this.fileName))
            {
                newTitle += "Untitled";
                this.Title = newTitle;
                return;
            }

            newTitle += Path.GetFileName(this.fileName);

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
            this.NewCommand = new CommandHandler(FileNewAction);
            this.OpenCommand = new CommandHandler(FileOpenAction);
            this.SaveCommand = new CommandHandler(FileSaveAction);
            this.SaveAsCommand = new CommandHandler(FileSaveAsAction);
            this.ExitCommand = new CommandHandler(FileExitAction);
            this.SolveCommand = new CommandHandler(ModelSolveAction);
            this.AddSingletonVariableCommand = new CommandHandler(ModelAddSingletonVariableAction, _ => CanAddSingletonVariableExecute);
            this.AddAggregateVariableCommand = new CommandHandler(ModelAddAggregateVariableAction, _ => CanAddAggregateVariableExecute);
            this.AddConstraintCommand = new CommandHandler(ModelAddConstraintAction);
            this.AddDomainCommand = new CommandHandler(ModelAddDomainAction, _ => CanAddDomainExecute);
            this.DeleteCommand = new CommandHandler(ModelDeleteAction, _ => CanDeleteExecute);
            this.ResizeCommand = new CommandHandler(ModelResizeAction, _ => CanResizeExecute);
        }
    }
}
