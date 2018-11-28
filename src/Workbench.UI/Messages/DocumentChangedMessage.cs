using System;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench.Messages
{
    /// <summary>
    /// Base for all document messages.
    /// </summary>
    public abstract class DocumentChangedMessage
    {
        public WorkspaceDocumentViewModel Document { get; }

        /// <summary>
        /// Initialize the document message with a document.
        /// </summary>
        /// <param name="theDocumentViewModel">Document view model</param>
        protected DocumentChangedMessage(WorkspaceDocumentViewModel theDocumentViewModel)
        {
            Contract.Requires<ArgumentNullException>(theDocumentViewModel != null);
            Document = theDocumentViewModel;
        }
    }
}