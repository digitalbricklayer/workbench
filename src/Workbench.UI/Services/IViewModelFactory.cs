using Workbench.ViewModels;

namespace Workbench.Services
{
    public interface IViewModelFactory
    {
        /// <summary>
        /// Create a new workspace view model.
        /// </summary>
        /// <returns>New workspace view model.</returns>
        WorkspaceViewModel CreateWorkspace();

        /// <summary>
        /// Create a new workspace document view model.
        /// </summary>
        /// <returns></returns>
        WorkspaceDocumentViewModel CreateDocument();

        /// <summary>
        /// Create a new model editor view model.
        /// </summary>
        /// <returns>New model editor view model.</returns>
        ModelEditorTabViewModel CreateModelEditor();

        /// <summary>
        /// Create a new bundle editor view model.
        /// </summary>
        /// <returns>A new bundle editor.</returns>
        BundleEditorViewModel CreateBundleEditor();
    }
}