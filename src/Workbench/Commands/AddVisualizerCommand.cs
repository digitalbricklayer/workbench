using System;
using System.Windows;
using System.Windows.Input;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Commands
{
    /// <summary>
    /// Add a new visualizer to the solution designer.
    /// </summary>
    public class AddVisualizerCommand : CommandBase
    {
        private readonly WorkspaceViewModel workspace;
        private readonly TitleBarViewModel titleBar;

        /// <summary>
        /// Initialize an add visualizer command with a workspace and titlebar.
        /// </summary>
        /// <param name="theWorkspace">Workspace view model.</param>
        /// <param name="theTitleBar">Title bar view model.</param>
        public AddVisualizerCommand(WorkspaceViewModel theWorkspace, TitleBarViewModel theTitleBar)
        {
            if (theWorkspace == null)
                throw new ArgumentNullException("theWorkspace");

            if (theTitleBar == null)
                throw new ArgumentNullException("theTitleBar");

            this.workspace = theWorkspace;
            this.titleBar = theTitleBar;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. 
        /// If the command does not require data to be passed, this object can be set to null.</param>
        public override void Execute(object parameter)
        {
            var newVisualizerLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.workspace.AddVisualizer(new VariableVisualizerViewModel(new VariableVisualizerModel(newVisualizerLocation)));
            this.titleBar.UpdateTitle();
        }
    }
}
