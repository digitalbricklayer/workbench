namespace Workbench.ViewModels
{
    /// <summary>
    /// Contract for the workspace.
    /// </summary>
    public interface IWorkspace
    {
        /// <summary>
        /// Get the workspace.
        /// </summary>
        /// <returns>Workspace view model.</returns>
        WorkspaceViewModel GetWorkspace();

        /// <summary>
        /// Get the model.
        /// </summary>
        /// <returns>Model view model.</returns>
        ModelViewModel GetModel();
    }
}