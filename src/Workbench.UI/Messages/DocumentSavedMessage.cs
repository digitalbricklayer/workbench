namespace Workbench.Messages
{
    public sealed class DocumentSavedMessage : DocumentChangedMessage
    {
        public DocumentSavedMessage(IWorkspaceDocument theDocumentViewModel)
            : base(theDocumentViewModel)
        {
        }
    }
}