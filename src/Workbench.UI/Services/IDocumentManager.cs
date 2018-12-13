using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Contract for the document manager.
    /// </summary>
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
    }
}
