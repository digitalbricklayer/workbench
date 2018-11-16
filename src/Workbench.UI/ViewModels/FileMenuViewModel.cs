using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public sealed class FileMenuViewModel : Screen
    {
        private readonly IDataService dataService;
        private readonly IAppRuntime appRuntime;
        private readonly TitleBarViewModel titleBar;
        private readonly IViewModelFactory viewModelFactory;

        public FileMenuViewModel(IDataService theDataService,
                                 IAppRuntime theAppRuntime,
                                 TitleBarViewModel theTitleBarViewModel,
                                 IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);

            this.dataService = theDataService;
            this.appRuntime = theAppRuntime;
            this.titleBar = theTitleBarViewModel;
            this.viewModelFactory = theViewModelFactory;
            NewCommand = new CommandHandler(FileNewAction);
            OpenCommand = new CommandHandler(FileOpenAction);
            SaveCommand = new CommandHandler(FileSaveAction);
            SaveAsCommand = new CommandHandler(FileSaveAsAction);
            ExitCommand = new CommandHandler(FileExitAction);
        }

        /// <summary>
        /// Gets the work area view model.
        /// </summary>
        public WorkspaceViewModel Workspace
        {
            get { return this.appRuntime.Workspace; }
            set { this.appRuntime.Workspace = value; }
        }

        public WorkspaceDocumentViewModel CurrentDocument => this.appRuntime.CurrentDocument;

        /// <summary>
        /// Gets the shell view model.
        /// </summary>
        public ShellViewModel Shell
        {
            get
            {
                return this.appRuntime.Shell;
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
        /// Handle the "File|New" menu item.
        /// </summary>
        private void FileNewAction()
        {
            var newDocument = this.viewModelFactory.CreateDocument();
            newDocument.New();
        }

        /// <summary>
        /// Handle the "File|Open" menu item.
        /// </summary>
        private void FileOpenAction()
        {
            CurrentDocument.Open();
        }

        /// <summary>
        /// Handle the "File|Save" menu item.
        /// </summary>
        private void FileSaveAction()
        {
            CurrentDocument.Save();
        }

        /// <summary>
        /// Handle the "File|Save As" menu item.
        /// </summary>
        private void FileSaveAsAction()
        {
            CurrentDocument.SaveAs();
        }

        /// <summary>
        /// Handle the "File|Exit" menu item.
        /// </summary>
        private void FileExitAction()
        {
            if (!CurrentDocument.TrySave())
            {
                return;
            }

            appRuntime.Shell.Close();
        }
    }
}