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
            this.AddVisualizerCommand = IoC.Get<AddVisualizerCommand>();
        }

        /// <summary>
        /// Gets the Solution|Add Visualizer command.
        /// </summary>
        public ICommand AddVisualizerCommand { get; private set; }
    }
}
