using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Commands;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution main menu.
    /// </summary>
    public class SolutionMenuViewModel
    {
        /// <summary>
        /// Initialize the solution menu view model with default values.
        /// </summary>
        public SolutionMenuViewModel()
        {
            AddChessboardVisualizerCommand = IoC.Get<AddChessboardVisualizerCommand>();
            AddMapVisualizerCommand = IoC.Get<AddMapVisualizerCommand>();
            EditMapVisualizerCommand = IoC.Get<EditMapCommand>();
            EditSolutionCommand = IoC.Get<EditSolutionCommand>();
        }

        /// <summary>
        /// Gets the Solution|Add Map command
        /// </summary>
        public ICommand AddMapVisualizerCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Add Chessboard command.
        /// </summary>
        public ICommand AddChessboardVisualizerCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Edit Solution command.
        /// </summary>
        public ICommand EditSolutionCommand { get; private set; }

        /// <summary>
        /// Gets the Solution|Edit Map command
        /// </summary>
        public ICommand EditMapVisualizerCommand { get; private set; }
    }
}
