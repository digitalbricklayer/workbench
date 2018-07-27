using Caliburn.Micro;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Factory for creating view models.
    /// </summary>
    public sealed class ViewModelFactory : IViewModelFactory
    {
        /// <inheritdoc />
        public WorkspaceViewModel CreateWorkspace()
        {
            var newWorkspace = IoC.Get<WorkspaceViewModel>();
			
			return newWorkspace;
        }

        /// <inheritdoc />
        public ModelEditorTabViewModel CreateModelEditor()
        {
            var newModelEditor = IoC.Get<ModelEditorTabViewModel>();

            return newModelEditor;
        }
    }
}
