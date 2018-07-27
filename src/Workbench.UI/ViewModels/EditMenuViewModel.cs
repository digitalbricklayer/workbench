using System;
using System.Diagnostics.Contracts;

namespace Workbench.ViewModels
{
    public sealed class EditMenuViewModel
    {
        private readonly IAppRuntime appRuntime;

        public EditMenuViewModel(IAppRuntime theAppRuntime)
        {
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);

            this.appRuntime = theAppRuntime;
        }

        /// <summary>
        /// Gets the work area view model.
        /// </summary>
        public WorkspaceViewModel Workspace
        {
            get { return this.appRuntime.Workspace; }
        }
    }
}
