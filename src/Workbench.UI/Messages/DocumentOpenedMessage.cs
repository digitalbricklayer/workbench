namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a document is opened. The document may be a new
    /// document with no contents or it may have the contents of a previously
    /// saved document.
    /// </summary>
    public class DocumentOpenedMessage : DocumentChangedMessage
    {
        /// <summary>
        /// Initialize a document opened message with an instance of the document that was opened.
        /// </summary>
        /// <param name="theDocumentViewModel"></param>
        public DocumentOpenedMessage(IWorkspaceDocument theDocumentViewModel)
            : base(theDocumentViewModel)
        {
        }
    }
}