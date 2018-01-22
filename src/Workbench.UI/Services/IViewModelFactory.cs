using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
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

        /// <summary>
        /// Event fired when a new work area view model is created.
        /// </summary>
		event EventHandler<WorkAreaCreatedArgs> WorkAreaCreated;
    }

    public class WorkAreaCreatedArgs
    {
        public WorkAreaCreatedArgs(WorkAreaViewModel theWorkArea)
        {
            Contract.Requires<ArgumentNullException>(theWorkArea != null);
            WorkAreaCreated = theWorkArea;
        }

        public WorkAreaViewModel WorkAreaCreated { get; private set; }
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

        public event EventHandler<WorkAreaCreatedArgs> WorkAreaCreated;
    }
}