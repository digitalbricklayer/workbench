using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Load the workspace view model from the workspace model.
    /// </summary>
    public class WorkspaceLoader : IWorkspaceLoader
    {
        private readonly ModelEditorLoader _modelEditorLoader;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IWindowManager _windowManager;

        /// <summary>
        /// Initialize the workspace loader with a model editor loader, view model factory and window manager.
        /// </summary>
        public WorkspaceLoader(ModelEditorLoader theModelEditorLoader, IViewModelFactory theViewModelFactory, IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theModelEditorLoader != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            _modelEditorLoader = theModelEditorLoader;
            _viewModelFactory = theViewModelFactory;
            _windowManager = theWindowManager;
        }

        /// <summary>
        /// Map a workspace model to a workspace view model.
        /// </summary>
        /// <param name="theWorkspaceModel">Workspace model.</param>
        /// <returns>Workspace view model.</returns>
        public WorkspaceViewModel Load(WorkspaceModel theWorkspaceModel)
        {
            var workspaceViewModel = _viewModelFactory.CreateWorkspace();
            workspaceViewModel.ModelEditor = _modelEditorLoader.LoadFrom(theWorkspaceModel);
            new VisualizerBindingLoader(workspaceViewModel).LoadFrom(theWorkspaceModel);
            new VisualizerLoader(workspaceViewModel, _windowManager).LoadFrom(theWorkspaceModel);

            return workspaceViewModel;
        }
    }
}
