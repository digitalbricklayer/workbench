using System.Windows.Input;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the 'Model' menu.
    /// </summary>
    public class ModelMenuViewModel : MenuViewModel
    {
        public ModelMenuViewModel()
        {
            SolveCommand = new CommandHandler(ModelSolveAction);
        }

        /// <summary>
        /// Gets the Model|Solve command
        /// </summary>
        public ICommand SolveCommand { get; }

        /// <summary>
        /// Solve the model.
        /// </summary>
        private void ModelSolveAction()
        {
            Workspace.SolveModel();
        }
    }
}