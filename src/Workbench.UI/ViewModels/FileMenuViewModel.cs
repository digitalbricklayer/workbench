using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public sealed class FileMenuViewModel : MenuViewModel
    {
        private readonly IDocumentManager _documentManager;

        public FileMenuViewModel(IDocumentManager theDocumentManager)
        {
            Contract.Requires<ArgumentNullException>(theDocumentManager != null);

            _documentManager = theDocumentManager;
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
            if (!Shell.CloseDocument()) return;
            var newDocument = _documentManager.CreateDocument();
            newDocument.New();
            Shell.OpenDocument(newDocument);
        }

        /// <summary>
        /// Handle the "File|Open" menu item.
        /// </summary>
        private void FileOpenAction()
        {
            if (!Shell.CloseDocument()) return;
            var newDocument = _documentManager.CreateDocument();
            newDocument.Open();
            Shell.OpenDocument(newDocument);
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
            // If the document is in a pristine state with no changes, exit the application
            if (CurrentDocument.IsNew)
            {
                Shell.Close();
                return;
            }

            // Close will return false if the user cancels the document close
            if (!CurrentDocument.Close()) return;

            Shell.Close();
        }
    }
}