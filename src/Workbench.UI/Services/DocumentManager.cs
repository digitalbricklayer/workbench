using System;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Manage the single instance of the workspace document.
    /// </summary>
    public class DocumentManager : IDocumentManager
    {
        private WorkspaceDocumentViewModel _currentDocument;
        private readonly IViewModelFactory _viewModelFactory;

        /// <summary>
        /// Initialize a new document manager with a view model factory.
        /// </summary>
        /// <param name="theViewModelFactory">View model factory.</param>
        public DocumentManager(IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);
            _viewModelFactory = theViewModelFactory;
        }

        /// <summary>
        /// Gets or sets the current document.
        /// </summary>
        public WorkspaceDocumentViewModel CurrentDocument
        {
            get => _currentDocument;
            private set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _currentDocument = value;
            }
        }

        /// <summary>
        /// Create a new document.
        /// </summary>
        public WorkspaceDocumentViewModel CreateDocument()
        {
            CurrentDocument = _viewModelFactory.CreateDocument();

            return CurrentDocument;
        }

        /// <summary>
        /// Close the current document.
        /// </summary>
        public bool CloseDocument()
        {
            var closeStatus = CurrentDocument.Close();
            // Did the user cancel the close?
            if (!closeStatus) return false;
            _currentDocument = null;
            return true;
        }
    }
}
