using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Commands;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the Visualizers main menu.
    /// </summary>
    public class VisualizerMenuViewModel
    {
        /// <summary>
        /// Initialize the visualizer menu view model with default values.
        /// </summary>
        public VisualizerMenuViewModel()
        {
            AddChessboardVisualizerCommand = IoC.Get<AddChessboardVisualizerCommand>();
            EditSolutionCommand = IoC.Get<EditSolutionCommand>();
        }

        /// <summary>
        /// Gets the Solution|Add Chessboard Visualizers command.
        /// </summary>
        public ICommand AddChessboardVisualizerCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Edit command.
        /// </summary>
        public ICommand EditSolutionCommand { get; private set; }
    }
}
