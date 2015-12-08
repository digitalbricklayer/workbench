using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Dyna.Core.Models;
using DynaApp.Factories;
using DynaApp.Services;
using DynaApp.Views;
using Microsoft.Win32;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public sealed class MainWindowViewModel : PropertyChangedBase
    {
		private const string ProgramTitle = "Constraint Workbench";
        private string title = string.Empty;
        private WorkspaceViewModel workspace;
        private readonly DataService dataService;
        private readonly IWorkspaceReaderWriter workspaceReaderWriter;
        private string fileName = String.Empty;
        private readonly IViewModelFactory viewModelFactory;

        /// <summary>
        /// Initialize a main windows view model with default values.
        /// </summary>
        public MainWindowViewModel(DataService theDataService,
                                   IWorkspaceReaderWriter theWorkspaceReaderWriter,
                                   IViewModelFactory theViewModelFactory)
        {
            if (theDataService == null)
                throw new ArgumentNullException("theDataService");
            if (theWorkspaceReaderWriter == null)
                throw new ArgumentNullException("theWorkspaceReaderWriter");
            if (theViewModelFactory == null)
                throw new ArgumentNullException("theViewModelFactory");
            this.dataService = theDataService;
            this.workspaceReaderWriter = theWorkspaceReaderWriter;
            this.viewModelFactory = theViewModelFactory;
            this.Workspace = this.viewModelFactory.CreateWorkspace();
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
            this.dataService.Reset();
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
                var workspaceModel = this.workspaceReaderWriter.Read(openFileDialog.FileName);
                var workspaceMapper = new WorkspaceMapper(new ModelViewModelCache(), null);
                this.Workspace = workspaceMapper.MapFrom(workspaceModel);
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
            this.Workspace.SolveModel(Application.Current.MainWindow);
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

            var resizeViewModel = new AggregateResizeViewModel();
            var resizeWindow = new AggregateVariableResizeWindow
            {
                Owner = Application.Current.MainWindow,
                DataContext = resizeViewModel
            };
            var showDialogResult = resizeWindow.ShowDialog();

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
                this.workspaceReaderWriter.Write(file, this.dataService.GetWorkspace());
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
            this.NewCommand = new CommandHandler(FileNewAction, _ => CanFileNewExecute);
            this.OpenCommand = new CommandHandler(FileOpenAction, _ => CanFileOpenExecute);
            this.SaveCommand = new CommandHandler(FileSaveAction, _ => CanFileSaveExecute);
            this.SaveAsCommand = new CommandHandler(FileSaveAsAction, _ => CanFileSaveAsExecute);
            this.ExitCommand = new CommandHandler(FileExitAction);
            this.SolveCommand = new CommandHandler(ModelSolveAction, _ => CanModelSolveExecute);
            this.AddSingletonVariableCommand = new CommandHandler(ModelAddSingletonVariableAction, _ => CanAddSingletonVariableExecute);
            this.AddAggregateVariableCommand = new CommandHandler(ModelAddAggregateVariableAction, _ => CanAddAggregateVariableExecute);
            this.AddConstraintCommand = new CommandHandler(ModelAddConstraintAction, _ => CanAddConstraintExecute);
            this.AddDomainCommand = new CommandHandler(ModelAddDomainAction, _ => CanAddDomainExecute);
            this.DeleteCommand = new CommandHandler(ModelDeleteAction, _ => CanDeleteExecute);
            this.ResizeCommand = new CommandHandler(ModelResizeAction, _ => CanResizeExecute);
        }
    }
}
