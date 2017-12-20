using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Factory for creating view models.
    /// </summary>
    public sealed class ViewModelFactory : IViewModelFactory
    {
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        public ViewModelFactory(IEventAggregator theEventAggregator, IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            this.eventAggregator = theEventAggregator;
            this.windowManager = theWindowManager;
        }

        /// <summary>
        /// Event fired when a new workspace view model is created.
        /// </summary>
        public event EventHandler<WorkspaceCreatedArgs> WorkspaceCreated;

        /// <summary>
        /// Event fired when a new model view model is created.
        /// </summary>
        public event EventHandler<ModelCreatedArgs> ModelCreated;

        /// <summary>
        /// Create a new workspace view model.
        /// </summary>
        /// <returns>New workspace view model.</returns>
        public WorkspaceViewModel CreateWorkspace()
        {
            var newWorkspace = IoC.Get<WorkspaceViewModel>();
			this.OnWorkspaceCreated(new WorkspaceCreatedArgs(newWorkspace));
			
			return newWorkspace;
        }

        /// <summary>
        /// Create a new model view model.
        /// </summary>
        /// <returns>New model view model.</returns>
        public ModelViewModel CreateModel(ModelModel theModel)
        {
            var newModel = new ModelViewModel(theModel,
                                              this.windowManager,
                                              this.eventAggregator);
            this.OnModelCreate(new ModelCreatedArgs(newModel));

            return newModel;
        }

        private void OnWorkspaceCreated(WorkspaceCreatedArgs e)
		{
			if (this.WorkspaceCreated != null)
			{
				WorkspaceCreated(this, e);
			}
		}

        private void OnModelCreate(ModelCreatedArgs e)
        {
            if (this.ModelCreated != null)
            {
                ModelCreated(this, e);
            }
        }
    }
}
