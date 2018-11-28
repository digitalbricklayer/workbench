using Workbench.ViewModels;

namespace Workbench.Services
{
    public interface IDocumentManager
    {
        /// <summary>
        /// Gets the current document.
        /// </summary>
        WorkspaceDocumentViewModel CurrentDocument { get; }

        /// <summary>
        /// Create a new document.
        /// </summary>
        WorkspaceDocumentViewModel CreateDocument();

        /// <summary>
        /// Close a document.
        /// </summary>
        /// <returns>True if the document was successfully closed or False if the user cancelled the operation.</returns>
        bool CloseDocument();
    }
}
