using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench.Services
{
    [ContractClass(typeof(IViewModelFactoryContract))]
    public interface IViewModelFactory
    {
        /// <summary>
        /// Create a new work area view model.
        /// </summary>
        /// <returns>New workspace view model.</returns>
        WorkspaceViewModel CreateWorkArea();

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
        public WorkspaceViewModel CreateWorkArea()
        {
            Contract.Ensures(Contract.Result<WorkspaceViewModel>() != null);
            return default(WorkspaceViewModel);
        }

        public ModelEditorTabViewModel CreateModelEditor()
        {
            Contract.Ensures(Contract.Result<ModelEditorTabViewModel>() != null);
            return default(ModelEditorTabViewModel);
        }
    }
}