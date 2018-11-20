using Workbench.ViewModels;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a document is opened.
    /// </summary>
    public class DocumentOpenedMessage : DocumentChangedMessage
    {
        public DocumentOpenedMessage(WorkspaceDocumentViewModel theDocumentViewModel)
            : base(theDocumentViewModel)
        {
        }
    }
}