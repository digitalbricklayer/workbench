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
        WorkAreaViewModel CreateWorkArea();
    }

    /// <summary>
    /// Code contract for the IViewModelFactory interface.
    /// </summary>
    [ContractClassFor(typeof(IViewModelFactory))]
    internal abstract class IViewModelFactoryContract : IViewModelFactory
    {
        public WorkAreaViewModel CreateWorkArea()
        {
            Contract.Ensures(Contract.Result<WorkAreaViewModel>() != null);
            return default(WorkAreaViewModel);
        }
    }
}