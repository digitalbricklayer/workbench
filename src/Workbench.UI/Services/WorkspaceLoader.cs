using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a model into a view model.
    /// </summary>
    public class WorkspaceLoader : IWorkspaceLoader
    {
        private readonly ModelEditorLoader _modelEditorLoader;
        private readonly IViewModelFactory viewModelFactory;

        /// <summary>
        /// Initialize the model editor loader with a window manager and view model factory.
        /// </summary>
        public WorkspaceLoader(ModelEditorLoader theModelEditorLoader, IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theModelEditorLoader != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);

            this._modelEditorLoader = theModelEditorLoader;
            this.viewModelFactory = theViewModelFactory;
        }

        /// <summary>
        /// Map a workspace model to a workspace view model.
        /// </summary>
        /// <param name="theWorkspaceModel">Workspace model.</param>
        /// <returns>Workspace view model.</returns>
        public WorkspaceViewModel Load(WorkspaceModel theWorkspaceModel)
        {
            var workAreaViewModel = this.viewModelFactory.CreateWorkspace();
            workAreaViewModel.ModelEditor = _modelEditorLoader.MapFrom(theWorkspaceModel);

            return workAreaViewModel;
        }
    }
}