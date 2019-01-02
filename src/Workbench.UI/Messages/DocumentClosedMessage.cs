namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a document is closed.
    /// </summary>
    public sealed class DocumentClosedMessage : DocumentChangedMessage
    {
        public DocumentClosedMessage(IWorkspaceDocument theDocumentViewModel)
            : base(theDocumentViewModel)
        {
        }
    }
}