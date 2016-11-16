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
            AddGridVisualizerCommand = IoC.Get<AddMapVisualizerCommand>();
            EditGridVisualizerCommand = IoC.Get<EditGridCommand>();
            EditSolutionCommand = IoC.Get<EditSolutionCommand>();
        }

        /// <summary>
        /// Gets the Solution|Add Map command
        /// </summary>
        public ICommand AddGridVisualizerCommand { get; private set; }

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
        public ICommand EditGridVisualizerCommand { get; private set; }
    }
}
