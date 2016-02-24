using System;
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
        /// Event fired when a new workspace view model is created.
        /// </summary>
		event EventHandler<WorkspaceCreatedArgs> WorkspaceCreated;
    }

    public class WorkspaceCreatedArgs
    {
        public WorkspaceCreatedArgs(WorkspaceViewModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);
            this.WorkspaceCreated = theWorkspace;
        }

        public WorkspaceViewModel WorkspaceCreated { get; private set; }
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

        public event EventHandler<WorkspaceCreatedArgs> WorkspaceCreated;
    }
}