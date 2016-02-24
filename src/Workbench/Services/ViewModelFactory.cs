using System;
using Caliburn.Micro;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Factory for creating view models.
    /// </summary>
    public class ViewModelFactory : IViewModelFactory
    {
        /// <summary>
        /// Event fired when a new workspace view model is created.
        /// </summary>
        public event EventHandler<WorkspaceCreatedArgs> WorkspaceCreated;

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
		
		private void OnWorkspaceCreated(WorkspaceCreatedArgs e)
		{
			if (this.WorkspaceCreated != null)
			{
				WorkspaceCreated(this, e);
			}
		}
    }
}
