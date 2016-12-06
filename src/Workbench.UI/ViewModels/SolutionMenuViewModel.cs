using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Commands;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution main menu.
    /// </summary>
    public class SolutionMenuViewModel
    {
        private readonly WorkspaceViewModel workspace;
        private readonly IWindowManager windowManager;

        /// <summary>
        /// Initialize the solution menu view model with default values.
        /// </summary>
        public SolutionMenuViewModel(IWindowManager theWindowManager, WorkspaceViewModel theWorkspace)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theWorkspace != null);

            this.workspace = theWorkspace;
            this.windowManager = theWindowManager;
            AddChessboardVisualizerCommand = IoC.Get<AddChessboardVisualizerCommand>();
            AddGridVisualizerCommand = IoC.Get<AddMapVisualizerCommand>();
            EditGridVisualizerCommand = IoC.Get<EditGridCommand>();
            EditSolutionCommand = IoC.Get<EditSolutionCommand>();
            AddColumnCommand = new CommandHandler(AddColumnHandler);
        }

        /// <summary>
        /// Gets the Solution|Add Column command.
        /// </summary>
        public ICommand AddColumnCommand { get; private set; }

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

        private void AddColumnHandler()
        {
            var selectedMapVisualizers = this.workspace.Solution.GetSelectedGridVisualizers();
            if (!selectedMapVisualizers.Any()) return;
            var selectedMapVisualizer = selectedMapVisualizers.First();
            var columnNameEditor = new ColumnNameEditorViewModel();
            var result = this.windowManager.ShowDialog(columnNameEditor);
            if (result.GetValueOrDefault())
            {
                selectedMapVisualizer.AddColumn(new GridColumnModel(columnNameEditor.ColumnName));
            }
        }
    }
}
