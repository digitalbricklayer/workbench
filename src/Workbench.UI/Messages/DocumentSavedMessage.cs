using Workbench.ViewModels;

namespace Workbench.Messages
{
    public sealed class DocumentSavedMessage : DocumentChangedMessage
    {
        public DocumentSavedMessage(WorkspaceDocumentViewModel theDocumentViewModel)
            : base(theDocumentViewModel)
        {
        }
    }
}