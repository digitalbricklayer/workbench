using Caliburn.Micro;
using Workbench.ViewModels;

namespace Workbench
{
    /// <summary>
    /// Contract for a workspace document.
    /// </summary>
    public interface IWorkspaceDocument : IScreen
    {
        /// <summary>
        /// Gets the document new flag.
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// Gets the document dirty flag.
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Gets the workspace.
        /// </summary>
        IWorkspace Workspace { get; }

        /// <summary>
        /// Gets the document path.
        /// </summary>
        DocumentPathViewModel Path { get; }

        /// <summary>
        /// Create a new workspace document.
        /// </summary>
        void New();

        /// <summary>
        /// Open the workspace document.
        /// </summary>
        void Open();

        /// <summary>
        /// Close the document.
        /// </summary>
        /// <returns>True if the document was saved successfully, False if the
        /// save was cancelled by the user.</returns>
        bool Close();

        /// <summary>
        /// Save the document to a file.
        /// </summary>
        bool Save();

        /// <summary>
        /// Ask the user for a file to save the document to and then save the document to that file.
        /// </summary>
        /// <returns>True if the document was saved successfully, false if the user cancelled.</returns>
        bool SaveAs();
    }
}