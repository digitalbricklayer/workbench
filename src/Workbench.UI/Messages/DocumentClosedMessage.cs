namespace Workbench.Messages
{
    public sealed class DocumentClosedMessage : DocumentChangedMessage
    {
        public DocumentClosedMessage(IWorkspaceDocument theDocumentViewModel)
            : base(theDocumentViewModel)
        {
        }
    }
}