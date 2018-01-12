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
        /// Create a new workspace view model.
        /// </summary>
        /// <returns>New workspace view model.</returns>
        WorkAreaViewModel CreateWorkspace();

        /// <summary>
        /// Event fired when a new workspace view model is created.
        /// </summary>
		event EventHandler<WorkspaceCreatedArgs> WorkspaceCreated;

        /// <summary>
        /// Create a new model view model.
        /// </summary>
        /// <returns>New model view model.</returns>
        ModelViewModel CreateModel(ModelModel theModel);

        /// <summary>
        /// Event fired when a new model view model is created.
        /// </summary>
        event EventHandler<ModelCreatedArgs> ModelCreated;
    }

    public class ModelCreatedArgs
    {
        public ModelCreatedArgs(ModelViewModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.NewModel = theModel;
        }

        public ModelViewModel NewModel { get; private set; }
    }

    public class WorkspaceCreatedArgs
    {
        public WorkspaceCreatedArgs(WorkAreaViewModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);
            this.WorkspaceCreated = theWorkspace;
        }

        public WorkAreaViewModel WorkspaceCreated { get; private set; }
    }

    /// <summary>
    /// Code contract for the IViewModelFactory interface.
    /// </summary>
    [ContractClassFor(typeof(IViewModelFactory))]
    internal abstract class IViewModelFactoryContract : IViewModelFactory
    {
        public WorkAreaViewModel CreateWorkspace()
        {
            Contract.Ensures(Contract.Result<WorkAreaViewModel>() != null);
            return default(WorkAreaViewModel);
        }

        public event EventHandler<WorkspaceCreatedArgs> WorkspaceCreated;
        public ModelViewModel CreateModel(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Ensures(Contract.Result<ModelViewModel>() != null);
            return default(ModelViewModel);
        }

        public event EventHandler<ModelCreatedArgs> ModelCreated;
    }
}