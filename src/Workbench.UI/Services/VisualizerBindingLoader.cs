using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    internal sealed class VisualizerBindingLoader
    {
        private readonly WorkspaceViewModel _workspaceViewModel;

        internal VisualizerBindingLoader(WorkspaceViewModel theWorkspaceViewModel)
        {
            _workspaceViewModel = theWorkspaceViewModel;
        }

        internal void LoadFrom(WorkspaceModel theWorkspaceModel)
        {
            foreach (var aBinding in theWorkspaceModel.Display.Bindings)
            {
                _workspaceViewModel.LoadBindingExpression(new VisualizerBindingExpressionViewModel(aBinding));
            }
        }
    }
}
