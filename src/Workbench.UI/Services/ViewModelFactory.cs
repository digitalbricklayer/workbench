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
        public WorkspaceViewModel CreateWorkArea()
        {
            var newWorkArea = IoC.Get<WorkspaceViewModel>();
			
			return newWorkArea;
        }

        /// <inheritdoc />
        public ModelEditorTabViewModel CreateModelEditor()
        {
            var newModelEditor = IoC.Get<ModelEditorTabViewModel>();

            return newModelEditor;
        }
    }
}
