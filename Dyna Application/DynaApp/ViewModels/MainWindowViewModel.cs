using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DynaApp.Models;
using DynaApp.Services;
using Microsoft.Win32;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the main window.
    /// </summary>
    public sealed class MainWindowViewModel : AbstractViewModel
    {
        private string filename;
        private string title;
        private WorkspaceViewModel worksapce;
        private readonly ModelService modelService = new ModelService();

        /// <summary>
        /// Initialize a main windows view model with default values.
        /// </summary>
        public MainWindowViewModel()
        {
            this.filename = string.Empty;
            this.Workspace = new WorkspaceViewModel();
            this.UpdateTitle();
            this.NewCommand = new CommandHandler(FileNewAction, CanFileNewExecute);
            this.OpenCommand = new CommandHandler(FileOpenAction, CanFileOpenExecute);
            this.SaveCommand = new CommandHandler(FileSaveAction, CanFileSaveExecute);
            this.SaveAsCommand = new CommandHandler(FileSaveAsAction, CanFileSaveAsExecute);
            this.ExitCommand = new CommandHandler(FileExitAction, CanFileExitExecute);
            this.SolveCommand = new CommandHandler(ModelSolveAction, CanModelSolveExecute);
            this.AddVariableCommand = new CommandHandler(ModelAddVariableAction, CanAddVariableExecute);
            this.AddConstraintCommand = new CommandHandler(ModelAddConstraintAction, CanAddConstraintExecute);
            this.AddDomainCommand = new CommandHandler(ModelAddDomainAction, CanAddDomainExecute);
        }

        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        public WorkspaceViewModel Workspace
        {
            get { return this.worksapce; }
            set
            {
                this.worksapce = value;
                OnPropertyChanged("Workspace");
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
        /// Gets whether the "Model|Add Variable" menu item can be executed.
        /// </summary>
        public bool CanAddVariableExecute
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

        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand SolveCommand { get; private set; }
        public ICommand AddVariableCommand { get; private set; }
        public ICommand AddConstraintCommand { get; private set; }
        public ICommand AddDomainCommand { get; private set; }

        /// <summary>
        /// Gets or sets the main window title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
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
                Filter = "XML files (*.xml)|*.xml|All Files|*.*",
                DefaultExt = "xml",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog().GetValueOrDefault() != true)
            {
                return;
            }

            this.Workspace.Reset();

            try
            {
                // Load file
                var workspaceReader = new WorkspaceReader(openFileDialog.FileName);
                var theWorkspaceModel = workspaceReader.Read();
                this.Workspace = this.modelService.MapFrom(theWorkspaceModel);
            }
            catch (Exception e)
            {
                this.ShowError(e.Message);
                return;
            }

            this.filename = openFileDialog.FileName;
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
                Filter = "XML files (*.xml)|*.xml|All Files|*.*",
                OverwritePrompt = true,
                DefaultExt = "xml",
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
            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// Handle the "Model|Solve" menu item.
        /// </summary>
        private void ModelSolveAction()
        {
            this.Workspace.SolveModel(Application.Current.MainWindow);
        }

        /// <summary>
        /// Event raised to create a new variable.
        /// </summary>
        private void ModelAddVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.CreateVariable("New Variable", newVariableLocation);
        }

        /// <summary>
        /// Event raised to create a new constraint.
        /// </summary>
        private void ModelAddConstraintAction()
        {
            var newConstraintLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.CreateConstraint("New Constraint", newConstraintLocation);
        }

        /// <summary>
        /// Event raised to create a new domain.
        /// </summary>
        private void ModelAddDomainAction()
        {
            var newDomainLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.CreateDomain("New Domain", newDomainLocation);
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
        /// Update main window title.
        /// </summary>
        private void UpdateTitle()
        {
            var s = "Dyna Project" + " - ";

            if (string.IsNullOrEmpty(this.filename))
            {
                s += "Untitled";
            }
            else
            {
                s += Path.GetFileName(this.filename);
            }

            if (this.Workspace.IsDirty)
            {
                s += " *";
            }

            this.Title = s;
        }

        /// <summary>
        /// Save the content to file.
        /// </summary>
        private bool Save(string file)
        {
            try
            {
                // Save file
                var workspaceWriter = new WorkspaceWriter(file);
                var workspaceModel = this.modelService.MapFrom(this.Workspace);
                workspaceWriter.Write(workspaceModel);
            }
            catch (Exception e)
            {
                this.ShowError(e.Message);
                return false;
            }

            this.filename = file;
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
                            "Dyna Project",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }
}
