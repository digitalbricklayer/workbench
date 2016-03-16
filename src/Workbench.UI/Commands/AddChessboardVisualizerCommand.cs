using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Input;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Commands
{
    /// <summary>
    /// Add a new chessboard visualizer to the solution designer.
    /// </summary>
    public class AddChessboardVisualizerCommand : CommandBase
    {
        private readonly WorkspaceViewModel workspace;
        private readonly TitleBarViewModel titleBar;

        public AddChessboardVisualizerCommand(WorkspaceViewModel theWorkspace,
                                              TitleBarViewModel theTitleBar)
        {
            Contract.Requires<ArgumentNullException>(theWorkspace != null);
            Contract.Requires<ArgumentNullException>(theTitleBar != null);

            this.workspace = theWorkspace;
            this.titleBar = theTitleBar;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. 
        /// If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public override void Execute(object parameter)
        {
            var newVisualizerLocation = Mouse.GetPosition(Application.Current.MainWindow);
            var newVisualizerModel = new ChessboardVisualizerModel(newVisualizerLocation);
            this.CreateDesigner(newVisualizerModel);
            this.CreateViewer(newVisualizerModel);
            this.titleBar.UpdateTitle();
        }

        private void CreateViewer(ChessboardVisualizerModel newVisualizerModel)
        {
            var visualizerViewerViewModel = new ChessboardVisualizerViewerViewModel(newVisualizerModel);
            this.workspace.AddViewer(visualizerViewerViewModel);
        }

        private void CreateDesigner(ChessboardVisualizerModel newVisualizerModel)
        {
            var visualizerDesignViewModel = new ChessboardVisualizerDesignViewModel(newVisualizerModel);
            this.workspace.AddDesigner(visualizerDesignViewModel);
        }
    }
}
