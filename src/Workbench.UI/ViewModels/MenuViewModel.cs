using Caliburn.Micro;

namespace Workbench.ViewModels
{
    public abstract class MenuViewModel : Screen
    {
        public WorkspaceDocumentViewModel CurrentDocument => Shell.CurrentDocument;

        /// <summary>
        /// Gets the shell view model.
        /// </summary>
        public IShell Shell => IoC.Get<IShell>();

        /// <summary>
        /// Gets the workspace view model.
        /// </summary>
        public WorkspaceViewModel Workspace => Shell.Workspace;
    }
}