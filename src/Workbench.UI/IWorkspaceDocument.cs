using Caliburn.Micro;
using Workbench.ViewModels;

namespace Workbench
{
    /// <summary>
    /// Contract for a workspace document.
    /// </summary>
    public interface IWorkspaceDocument : IScreen
    {
        bool IsNew { get; }
        bool IsDirty { get; }
        IWorkspace Workspace { get; }

        /// <summary>
        /// Gets the document path.
        /// </summary>
        DocumentPathViewModel Path { get; }

        void New();

        void Open();

        bool Close();

        bool Save();

        bool SaveAs();
    }
}