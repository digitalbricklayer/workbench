using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Load visualizers from the model.
    /// </summary>
    internal sealed class VisualizerLoader
    {
        private readonly WorkspaceViewModel _workspaceViewModel;
        private readonly IWindowManager _windowManager;

        internal VisualizerLoader(WorkspaceViewModel theWorkspaceViewModel, IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theWorkspaceViewModel != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            _workspaceViewModel = theWorkspaceViewModel;
            _windowManager = theWindowManager;
        }

        internal void LoadFrom(WorkspaceModel theWorkspaceModel)
        {
            foreach (var aVisualizer in theWorkspaceModel.Display.Visualizers)
            {
                switch (aVisualizer)
                {
                    case ChessboardTabModel chessboardTab:
                        var newChessboardTab = new ChessboardTabViewModel(chessboardTab, _windowManager);
                        _workspaceViewModel.LoadChessboardTab(newChessboardTab);
                        break;

                    case TableTabModel _:
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}