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
            this.AddChessboardVisualizerCommand = IoC.Get<AddChessboardVisualizerCommand>();
        }

        /// <summary>
        /// Gets the Solution|Add Chessboard Visualizer command.
        /// </summary>
        public ICommand AddChessboardVisualizerCommand { get; private set; }
    }
}
