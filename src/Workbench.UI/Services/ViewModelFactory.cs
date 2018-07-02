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
        /// Create a new work area view model.
        /// </summary>
        /// <returns>New work area view model.</returns>
        public WorkAreaViewModel CreateWorkArea()
        {
            var newWorkArea = IoC.Get<WorkAreaViewModel>();
			
			return newWorkArea;
        }
    }
}
