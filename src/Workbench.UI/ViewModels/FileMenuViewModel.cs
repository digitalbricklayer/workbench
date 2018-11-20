using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public sealed class FileMenuViewModel : MenuViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;

        public FileMenuViewModel(IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);

            _viewModelFactory = theViewModelFactory;
            NewCommand = new CommandHandler(FileNewAction);
            OpenCommand = new CommandHandler(FileOpenAction);
            SaveCommand = new CommandHandler(FileSaveAction);
            SaveAsCommand = new CommandHandler(FileSaveAsAction);
            ExitCommand = new CommandHandler(FileExitAction);
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
            var newDocument = _viewModelFactory.CreateDocument();
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

            Shell.Close();
        }
    }
}