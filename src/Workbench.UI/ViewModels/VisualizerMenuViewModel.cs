using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Commands;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the Visualizer main menu.
    /// </summary>
    public class VisualizerMenuViewModel
    {
        /// <summary>
        /// Initialize the visualizer menu view model with default values.
        /// </summary>
        public VisualizerMenuViewModel()
        {
            this.AddVariableVisualizerCommand = IoC.Get<AddVariableVisualizerCommand>();
            this.AddChessboardVisualizerCommand = IoC.Get<AddChessboardVisualizerCommand>();
        }

        /// <summary>
        /// Gets the Solution|Add Variable Visualizer command.
        /// </summary>
        public ICommand AddVariableVisualizerCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Add Chessboard Visualizer command.
        /// </summary>
        public ICommand AddChessboardVisualizerCommand { get; private set; }
    }
}
