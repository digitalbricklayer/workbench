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
    public class AddVisualizerCommand : ICommand
    {
        public AddVisualizerCommand(WorkspaceViewModel theWorkspace, TitleBarViewModel theTitleBar)
        {
            this.Workspace = theWorkspace;
            this.TitleBar = theTitleBar;
        }

        public TitleBarViewModel TitleBar { get; private set; }

        public WorkspaceViewModel Workspace { get; private set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var newVisualizerLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddVisualizer(new VariableVisualizerViewModel(new VariableVisualizerModel(newVisualizerLocation)));
            this.TitleBar.UpdateTitle();
        }

        public event EventHandler CanExecuteChanged;
    }
}
