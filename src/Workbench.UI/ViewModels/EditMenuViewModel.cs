using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    public sealed class EditMenuViewModel : Screen
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
