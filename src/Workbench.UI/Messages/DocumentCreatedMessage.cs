using Workbench.ViewModels;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a new document is created.
    /// </summary>
    public sealed class DocumentCreatedMessage : DocumentChangedMessage
    {
        public DocumentCreatedMessage(WorkspaceDocumentViewModel theDocumentViewModel)
            : base(theDocumentViewModel)
        {
        }
    }
}