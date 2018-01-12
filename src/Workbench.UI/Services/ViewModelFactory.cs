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
        public event EventHandler<WorkAreaCreatedArgs> WorkAreaCreated;

        /// <summary>
        /// Event fired when a new model view model is created.
        /// </summary>
        public event EventHandler<ModelCreatedArgs> ModelCreated;

        /// <summary>
        /// Create a new work area view model.
        /// </summary>
        /// <returns>New work area view model.</returns>
        public WorkAreaViewModel CreateWorkArea()
        {
            var newWorkArea = IoC.Get<WorkAreaViewModel>();
			this.OnWorkAreaCreated(new WorkAreaCreatedArgs(newWorkArea));
			
			return newWorkArea;
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

        private void OnWorkAreaCreated(WorkAreaCreatedArgs e)
		{
			if (this.WorkAreaCreated != null)
			{
				WorkAreaCreated(this, e);
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
