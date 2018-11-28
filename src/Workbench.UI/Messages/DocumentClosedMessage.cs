using Workbench.ViewModels;

namespace Workbench.Messages
{
    public sealed class DocumentClosedMessage : DocumentChangedMessage
    {
        public DocumentClosedMessage(WorkspaceDocumentViewModel theDocumentViewModel)
            : base(theDocumentViewModel)
        {
        }
    }
}