using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench.Services
{
    [ContractClass(typeof(IViewModelFactoryContract))]
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
    }

    /// <summary>
    /// Code contract for the IViewModelFactory interface.
    /// </summary>
    [ContractClassFor(typeof(IViewModelFactory))]
    internal abstract class IViewModelFactoryContract : IViewModelFactory
    {
        public WorkspaceViewModel CreateWorkspace()
        {
            Contract.Ensures(Contract.Result<WorkspaceViewModel>() != null);
            return default(WorkspaceViewModel);
        }

        public WorkspaceDocumentViewModel CreateDocument()
        {
            Contract.Ensures(Contract.Result<WorkspaceDocumentViewModel>() != null);
            return default(WorkspaceDocumentViewModel);
        }

        public ModelEditorTabViewModel CreateModelEditor()
        {
            Contract.Ensures(Contract.Result<ModelEditorTabViewModel>() != null);
            return default(ModelEditorTabViewModel);
        }
    }
}