namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a document is saved.
    /// </summary>
    public sealed class DocumentSavedMessage : DocumentChangedMessage
    {
        public DocumentSavedMessage(IWorkspaceDocument theDocumentViewModel)
            : base(theDocumentViewModel)
        {
        }
    }
}