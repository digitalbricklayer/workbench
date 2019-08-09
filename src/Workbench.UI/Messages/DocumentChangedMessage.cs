namespace Workbench.Messages
{
    /// <summary>
    /// Base for all document messages.
    /// </summary>
    public abstract class DocumentChangedMessage
    {
        /// <summary>
        /// Gets the changed document.
        /// </summary>
        public IWorkspaceDocument Document { get; }

        /// <summary>
        /// Gets the workspace inside the changed document.
        /// </summary>
        public IWorkspace Workspace => Document.Workspace;

        /// <summary>
        /// Initialize the document message with a document.
        /// </summary>
        /// <param name="theDocumentViewModel">Document view model</param>
        protected DocumentChangedMessage(IWorkspaceDocument theDocumentViewModel)
        {
            Document = theDocumentViewModel;
        }
    }
}