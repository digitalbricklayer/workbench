using System;
using System.Diagnostics.Contracts;
using System.IO;
using Caliburn.Micro;
using Workbench.Messages;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Title bar in the main window.
    /// </summary>
    public class TitleBarViewModel : Screen, IHandle<DocumentChangedMessage>, IHandle<WorkspaceChangedMessage>
    {
        private string _title;
        private WorkspaceDocumentViewModel _currentDocument;

        /// <summary>
        /// Initialize a title bar view model with default values.
        /// </summary>
        public TitleBarViewModel(IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            theEventAggregator.Subscribe(this);
            Title = "Constraint Capers Workbench" + " - " + "Untitled";
        }

        /// <summary>
        /// Gets or sets the main window title.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _title = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Hand the document change messages.
        /// </summary>
        /// <param name="theMessage">Document changed message.</param>
        public void Handle(DocumentChangedMessage theMessage)
        {
            _currentDocument = theMessage.Document;
            UpdateTitle();
        }

        /// <summary>
        /// Handle workspace change messages.
        /// </summary>
        /// <param name="message">Change message.</param>
        public void Handle(WorkspaceChangedMessage message)
        {
            Contract.Assert(_currentDocument != null);
            UpdateTitle();
        }

        /// <summary>
        /// Update main window title.
        /// </summary>
        private void UpdateTitle()
        {
            var newTitle = "Constraint Capers Workbench" + " - ";

            if (_currentDocument.IsNew)
            {
                newTitle += "Untitled";
                Title = newTitle;
                return;
            }

            newTitle += Path.GetFileName(_currentDocument.Path.FullPath);

            if (_currentDocument.IsDirty)
            {
                newTitle += " *";
            }

            Title = newTitle;
        }
    }
}